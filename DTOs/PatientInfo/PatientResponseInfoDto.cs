namespace AxonPDS.DTOs.PatientInfo;

public record class PatientResponseInfoDto
{
     public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public string BloodGroup { get; set; } = string.Empty;

    public string Weight { get; set; } = string.Empty;

    public string Height { get; set; } = string.Empty;

    public string Allergies { get; set; } = string.Empty;
    
    public string Habits { get; set; } = string.Empty;

    public string MedicalHistory { get; set; } = string.Empty;

}
