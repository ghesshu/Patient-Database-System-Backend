using System;
using AxonPDS.Interfaces;

namespace AxonPDS.Entities;

public class PatientInfo : IBaseEntity
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public Patient? Patient { get; set; }

    public String BloodGroup { get; set; } = string.Empty;

    public String Weight { get; set; } = string.Empty;

    public String Height { get; set; } = string.Empty;

    public String Allergies { get; set; } = string.Empty;
    
    public String Habits { get; set; } = string.Empty;

    public String Medicalhistory { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

};
