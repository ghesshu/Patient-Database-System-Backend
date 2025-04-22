using System.ComponentModel.DataAnnotations;

namespace AxonPDS.DTOs.Record;

public record class UpdateRecordDto
{
    [Required]
    public string Complains { get; set; } = string.Empty;

    [Required]
    public string Diagnosis { get; set; } = string.Empty;

    [Required]
    public string VitalSigns { get; set; } = string.Empty;

}
