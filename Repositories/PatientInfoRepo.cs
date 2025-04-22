using System;
using AxonPDS.Data;
using AxonPDS.DTOs.PatientInfo;
using AxonPDS.Entities;
using AxonPDS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AxonPDS.Repositories;

public class PatientInfoRepo(PdsDbContext pdsDbContext) : IPatientInfo
{
    private readonly PdsDbContext service = pdsDbContext;

    // Map PatientInfo entity to PatientResponseInfoDto
    private static PatientResponseInfoDto MapToDto(PatientInfo patientInfo)
    {
        return new PatientResponseInfoDto
        {
            Id = patientInfo.Id,
            PatientId = patientInfo.PatientId,
            BloodGroup = patientInfo.BloodGroup,
            Weight = patientInfo.Weight,
            Height = patientInfo.Height,
            Allergies = patientInfo.Allergies,
            Habits = patientInfo.Habits,
            MedicalHistory = patientInfo.Medicalhistory // Fixed typo (Medicalhistory → MedicalHistory)
        };
    }


    // Private function to create a new PatientInfo with provided data
    private async Task<PatientInfo> CreateNewPatientInfoAsync(Guid patientId, CreatePatientInfoDto patientInfo)
    {
        var newPatientInfo = new PatientInfo
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            BloodGroup = patientInfo.BloodGroup ?? string.Empty,
            Weight = patientInfo.Weight ?? string.Empty,
            Height = patientInfo.Height ?? string.Empty,
            Allergies = patientInfo.Allergies ?? string.Empty,
            Habits = patientInfo.Habits ?? string.Empty,
            Medicalhistory = patientInfo.MedicalHistory ?? string.Empty,
        };

        await service.PatientInfos.AddAsync(newPatientInfo);
        await service.SaveChangesAsync();

        return newPatientInfo;
    }

    public async Task<PatientResponseInfoDto> GetPatientInfo(Guid patientId)
    {
        var patientInfo = await service.PatientInfos.FirstOrDefaultAsync(p => p.PatientId == patientId);

        if (patientInfo is null)
        {
            // Create a new PatientInfo with default values
            patientInfo = await CreateNewPatientInfoAsync(patientId, new CreatePatientInfoDto());

            // Update the corresponding Patient record with the new PatientInfoId
            var patient = await service.Patients.FirstOrDefaultAsync(p => p.Id == patientId);
            if (patient is not null)
            {
                patient.PatientInfoId = patientInfo.Id;
                service.Patients.Update(patient);
                await service.SaveChangesAsync();
            }
        }

        return MapToDto(patientInfo);
    }

    public async Task<PatientResponseInfoDto?> UpdatePatientInfo(Guid patientId, CreatePatientInfoDto patientInfo)
    {
        var existingInfo = await service.PatientInfos.FirstOrDefaultAsync(p => p.PatientId == patientId);

        if (existingInfo is null)
        {
            // Fetch the corresponding Patient record and update its PatientInfoId
            var patient = await service.Patients.FirstOrDefaultAsync(p => p.Id == patientId);
            if (patient is null)
            {
               return null;
            }
            
            // Create a new PatientInfo with the given data
            existingInfo = await CreateNewPatientInfoAsync(patientId, patientInfo);

            patient.PatientInfoId = existingInfo.Id;
            service.Patients.Update(patient);
            await service.SaveChangesAsync();
        }
        else
        {
            // Update existing record
            existingInfo.BloodGroup = patientInfo.BloodGroup ?? existingInfo.BloodGroup;
            existingInfo.Weight = patientInfo.Weight ?? existingInfo.Weight;
            existingInfo.Height = patientInfo.Height ?? existingInfo.Height;
            existingInfo.Allergies = patientInfo.Allergies ?? existingInfo.Allergies;
            existingInfo.Medicalhistory = patientInfo.MedicalHistory ?? existingInfo.Medicalhistory;
            existingInfo.UpdatedAt = DateTime.UtcNow;

            service.PatientInfos.Update(existingInfo);
            await service.SaveChangesAsync();
         }

        return MapToDto(existingInfo); // Now existingInfo is guaranteed to be non-null
    }

}
