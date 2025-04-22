using System;
using AxonPDS.DTOs.Record;
using AxonPDS.Entities;

namespace AxonPDS.DTOs.Patient
{
    public class PatientResponseDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
          public DateTime CreatedAt { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }

        public string? Phonenumber { get; set; } = string.Empty;
        public string? Emergency { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public DateTime? Dob { get; set; }
        public string? Email { get; set; } = string.Empty;

        public PatientInfoDto? Info { get; set; } // Nullable to make it optional

        public List<RecordResponseDto?> Records { get; set; } = new();
    }

    public class PatientInfoDto
    {
        public string BloodGroup { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Allergies { get; set; } = string.Empty;
        public string Habits { get; set; } = string.Empty;
        public string MedicalHistory { get; set; } = string.Empty;
    }
}