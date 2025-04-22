using System;
using System.ComponentModel.DataAnnotations;
using AxonPDS.Interfaces;

namespace AxonPDS.Entities;

public class Medicine : IBaseEntity
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int Stock { get; set; } 

    [Required]
    public string Measure {get; set;}  = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<MedicineRecord> MedicineRecords { get; set; } = [];

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}

// beauty in black 