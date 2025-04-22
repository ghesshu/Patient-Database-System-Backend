using System.ComponentModel.DataAnnotations;

namespace AxonPDS.DTOs.Medicine;

public record class UpdateMedicineDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Measure { get; set; } = string.Empty;

    [Required]
    public int Stock { get; set; }

    public string? Description { get; set; } = string.Empty;

}
