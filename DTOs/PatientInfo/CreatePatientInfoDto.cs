namespace AxonPDS.DTOs.PatientInfo;

public record class CreatePatientInfoDto
{
    public string? BloodGroup { get; set; }

    public string? Weight { get; set; }

    public string? Height { get; set; }

    public string? Allergies { get; set; }
    public string? Habits { get; set; }

    public string? MedicalHistory { get; set; }

}
