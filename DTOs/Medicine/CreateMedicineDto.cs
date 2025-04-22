using System.ComponentModel.DataAnnotations;

namespace AxonPDS.DTOs.Medicine;

public record class CreateMedicineDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string Measure { get; set; } = string.Empty;

    public int Stock { get; set; } 

    public string Description { get; set; } = string.Empty;

}
 