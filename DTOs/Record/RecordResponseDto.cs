namespace AxonPDS.DTOs.Record;

public record class RecordResponseDto
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public string Complains { get; set; } = string.Empty;

    public string Diagnosis { get; set; } = string.Empty;

    public string VitalSigns { get; set; } = string.Empty;

    public string[] Treatment { get; set; } = Array.Empty<string>();
    
    public MedicineRecordItem[] Medicines { get; set; } = Array.Empty<MedicineRecordItem>();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

