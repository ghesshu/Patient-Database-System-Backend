using System;
using System.ComponentModel.DataAnnotations;
using AxonPDS.Interfaces;

namespace AxonPDS.Entities;

public class TreatmentRecord : IBaseEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public Guid RecordId { get; set; }

    public Record? Record { get; set; } = null!;

    [Required]
    public Guid TreatmentId { get; set; }
    
    public Treatment? Treatment { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
