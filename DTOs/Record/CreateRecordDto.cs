using System.ComponentModel.DataAnnotations;

namespace AxonPDS.DTOs.Record;

public record class CreateRecordDto
{
    public Guid PatientId { get; set; }
    public string Complains { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string VitalSigns { get; set; } = string.Empty;
    public string[] Treatment { get; set; } = Array.Empty<string>();
    public MedicineRecordItem[] Medicines { get; set; } = Array.Empty<MedicineRecordItem>();
}

public class MedicineRecordItem
{
    public string Medicine { get; set; } = string.Empty;
    public string Instruction { get; set; } = string.Empty;
    public string Quantity { get; set; } = string.Empty;
}


