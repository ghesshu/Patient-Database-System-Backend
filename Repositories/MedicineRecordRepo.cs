using System;
using AxonPDS.Data;
using AxonPDS.DTOs.MedicineRecord;
using AxonPDS.Entities;
using AxonPDS.Interfaces;

namespace AxonPDS.Repositories;

public class MedicineRecordRepo (PdsDbContext pdsDbContext) : IMedicineRecord
{

    private readonly PdsDbContext service = pdsDbContext;


    public async Task<(bool, string)> CreateMRecord(CreateMedicineRecordDto dto)
    {
        var medicine = await service.Medicines.FindAsync(dto.MedicineID);
        if (medicine == null)
            return (false, "Medicine not found.");

        int quantityToDeduct = int.Parse(dto.Quantity);
        if (medicine.Stock < quantityToDeduct)
            return (false, "Insufficient stock for this medicine.");

        var medicineRecord = new MedicineRecord
        {
            Id = Guid.NewGuid(),
            RecordId = dto.RecordId,
            MedicineId = dto.MedicineID,
            Instruction = dto.Instruction,
            Quantity = dto.Quantity,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        service.MedicineRecords.Add(medicineRecord);
        
        // Deduct the quantity
        medicine.Stock -= quantityToDeduct;
        medicine.UpdatedAt = DateTime.UtcNow;

        await service.SaveChangesAsync();
        return (true, "Medicine record created successfully.");
    }

    public async Task<(bool, string)> DeleteMRecord(Guid id)
    {
        var medicineRecord = await service.MedicineRecords.FindAsync(id);
        if (medicineRecord == null)
            return (false, "Medicine record not found.");

        var medicine = await service.Medicines.FindAsync(medicineRecord.MedicineId);
        if (medicine != null)
        {
            int quantityToAdd = int.Parse(medicineRecord.Quantity);
            medicine.Stock += quantityToAdd;
            medicine.UpdatedAt = DateTime.UtcNow;
        }

        service.MedicineRecords.Remove(medicineRecord);
        await service.SaveChangesAsync();
        return (true, "Medicine record deleted successfully.");
    }
}