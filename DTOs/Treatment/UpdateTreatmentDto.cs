namespace AxonPDS.DTOs.Treatment;

public record class UpdateTreatmentDto
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int? Active { get; set; } 

}
