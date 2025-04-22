using System;
using AxonPDS.DTOs.PatientInfo;
using AxonPDS.Entities;

namespace AxonPDS.Interfaces;

public interface IPatientInfo
{
    Task<PatientResponseInfoDto> GetPatientInfo(Guid id);

    Task<PatientResponseInfoDto?> UpdatePatientInfo(Guid patientId, CreatePatientInfoDto patientInfo);

    
}
