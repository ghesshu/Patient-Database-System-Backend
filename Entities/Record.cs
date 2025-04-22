using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxonPDS.Entities;

public class Record
{
    [Key]
     public Guid Id { get; set; }

    [Required]
     public Guid PatientId { get; set; }

    // [ForeignKey("PatientId")]
     public Patient? Patient { get; set; }

    [Required]
    public string Complains { get; set; } = string.Empty;

    [Required]
    public string Diagnosis { get; set; } = string.Empty;

    [Required]
    public string VitalSigns { get; set; } = string.Empty;

    [Required]
     public List<TreatmentRecord> TreatmentRecords { get; set; } = [];

    [Required]
    public List<MedicineRecord> MedicineRecords { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


}
