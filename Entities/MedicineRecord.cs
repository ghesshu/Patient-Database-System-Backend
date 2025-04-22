using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AxonPDS.Interfaces;

namespace AxonPDS.Entities;

public class MedicineRecord : IBaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid RecordId { get; set; }


    public Record? Record { get; set; }

    [Required]
    public Guid MedicineId { get; set; }

    // [ForeignKey("MedicineId")]
    public Medicine? Medicine { get; set; }

    [Required]
    public string Instruction { get; set; } = string.Empty;

    [Required]
    public string Quantity { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


}
