namespace AxonPDS.DTOs.TreatmentRecord;

public record class CreateTRecordDTO
{

    public Guid RecordId { get; set; }

    public List<Guid> TreatmentIds { get; set; } = new();

}
