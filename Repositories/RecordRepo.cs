using System;
using AxonPDS.Data;
using AxonPDS.DTOs.Record;
using AxonPDS.Entities;
using AxonPDS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AxonPDS.Repositories;

public class RecordRepo(PdsDbContext pdsDbContext) : IRecord
{
    private readonly PdsDbContext service = pdsDbContext;

    public async Task<(bool Success, string Message, RecordResponseDto? Record)> CreateRecordAsync(CreateRecordDto dto)
    {
        using var transaction = await service.Database.BeginTransactionAsync();
        try
        {
            // First, check all medicine quantities
            foreach (var med in dto.Medicines)
            {
                var medicine = await service.Medicines.FindAsync(Guid.Parse(med.Medicine));
                if (medicine == null)
                    return (false, $"Medicine with ID {med.Medicine} not found.", null);

                int quantityNeeded = int.Parse(med.Quantity);
                if (medicine.Stock < quantityNeeded)
                    return (false, $"Insufficient stock for medicine {medicine.Name}. Available: {medicine.Stock}, Requested: {quantityNeeded}", null);
            }

            // Create the main record
            var record = new Record
            {
                Id = Guid.NewGuid(),
                PatientId = dto.PatientId,
                Complains = dto.Complains,
                Diagnosis = dto.Diagnosis,
                VitalSigns = dto.VitalSigns,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            service.Records.Add(record);

            // Add treatment records
            var treatmentRecords = dto.Treatment.Select(treatmentId => new TreatmentRecord
            {
                RecordId = record.Id,
                TreatmentId = Guid.Parse(treatmentId),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).ToList();

            await service.TreatmentRecords.AddRangeAsync(treatmentRecords);

            // Add medicine records and update stock
            foreach (var med in dto.Medicines)
            {
                var medicine = await service.Medicines.FindAsync(Guid.Parse(med.Medicine));
                var quantityToDeduct = int.Parse(med.Quantity);

                var medicineRecord = new MedicineRecord
                {
                    Id = Guid.NewGuid(),
                    RecordId = record.Id,
                    MedicineId = Guid.Parse(med.Medicine),
                    Instruction = med.Instruction,
                    Quantity = med.Quantity,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                medicine!.Stock -= quantityToDeduct;
                medicine.UpdatedAt = DateTime.UtcNow;

                await service.MedicineRecords.AddAsync(medicineRecord);
            }

            await service.SaveChangesAsync();
            await transaction.CommitAsync();

            var response = new RecordResponseDto
            {
                Id = record.Id,
                PatientId = record.PatientId,
                Complains = record.Complains,
                Diagnosis = record.Diagnosis,
                VitalSigns = record.VitalSigns,
                CreatedAt = record.CreatedAt,
                UpdatedAt = record.UpdatedAt
            };

            return (true, "Record created successfully with treatments and medicines.", response);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return (false, $"Error creating record: {ex.Message}", null);
        }
    }

    public async Task<(bool Success, string Message, RecordResponseDto? Record)> UpdateRecordAsync(Guid id, CreateRecordDto dto)
    {
        var record = await service.Set<Record>().FindAsync(id);
        if (record == null) return (false, "Record not found.", null);

        record.Complains = dto.Complains;
        record.Diagnosis = dto.Diagnosis;
        record.VitalSigns = dto.VitalSigns;
        record.UpdatedAt = DateTime.UtcNow;

        await service.SaveChangesAsync();

        var response = new RecordResponseDto
        {
            Id = record.Id,
            PatientId = record.PatientId,
            Complains = record.Complains,
            Diagnosis = record.Diagnosis,
            VitalSigns = record.VitalSigns,
        };

        return (true, "Record updated successfully.", response);
    }

    public async Task<(bool Success, string Message)> DeleteRecordAsync(Guid id)
    {
        var record = await service.Set<Record>().FindAsync(id);
        if (record == null) return (false, "Record not found.");

        service.Set<Record>().Remove(record);
        await service.SaveChangesAsync();
        
        return (true, "Record deleted successfully.");
    }

    public async Task<(bool Success, string Message, List<RecordResponseDto?> Records)> GetRecordsByPatientIdAsync(Guid patientId)
    {
        var records = await service.Set<Record>()
            .Include(r => r.TreatmentRecords)
            .Include(r => r.MedicineRecords)
            .Where(r => r.PatientId == patientId)
            .Select(r => new RecordResponseDto
            {
                Id = r.Id,
                PatientId = r.PatientId,
                Complains = r.Complains,
                Diagnosis = r.Diagnosis,
                VitalSigns = r.VitalSigns,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                Treatment = r.TreatmentRecords.Select(tr => tr.TreatmentId.ToString()).ToArray(),
                Medicines = r.MedicineRecords.Select(mr => new MedicineRecordItem
                {
                    Medicine = mr.MedicineId.ToString(),
                    Instruction = mr.Instruction,
                    Quantity = mr.Quantity
                }).ToArray()
            })
            .ToListAsync();

        return (true, "Records retrieved successfully.", records as List<RecordResponseDto?>);
    }

}