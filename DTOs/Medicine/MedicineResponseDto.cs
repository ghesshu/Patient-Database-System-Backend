namespace AxonPDS.DTOs.Medicine;

public record class MedicineResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Measure { get; set; } = string.Empty;

    public int Stock { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt {get; set;}

    public DateTime? UpdatedAt {get; set;}

}
