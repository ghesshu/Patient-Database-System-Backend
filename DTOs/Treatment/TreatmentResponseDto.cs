namespace AxonPDS.DTOs.Treatment;

public record class TreatmentResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Active { get; set; } 

    public DateTime CreatedAt { get; set; } 

}

