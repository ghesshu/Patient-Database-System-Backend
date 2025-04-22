using System;
using AxonPDS.DTOs.Patient;
using AxonPDS.Entities;

namespace AxonPDS.Interfaces;

public interface IPatient
{
    Task<PatientResponseDto?> GetPatient(Guid id);

    Task<(List<PatientResponseDto>, int)> GetAllPatients(
            string gender = "all",
            string? dateRange = null,
            string sort = "newest",
            int page = 1,
            int limit = 20,
            string search = "");

    Task<Patient?> CreatePatient(CreatePatientDto patient);

    Task<bool> UpdatePatient(Guid Id, UpdatePatientDto patient);
    
    Task<bool> DeletePatient(Guid Id);
    
    Task<bool> HasRecords(Guid Id);
    
    Task<bool> IsDuplicateAsync(string email, string phoneNumber);

}
