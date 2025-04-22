using System;

namespace AxonPDS.DTOs.MedicineRecord;

public class MedicineRecordResponseDto
{
    public Guid Id {get; set; }
    public Guid RecordId { get; set; }

    public Guid MedicineID {get; set;}

    public string Instruction { get; set; } = string.Empty;

    public string Quantity { get; set; } = string.Empty;

}
