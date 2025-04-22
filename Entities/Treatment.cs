using System;
using System.ComponentModel.DataAnnotations;
using AxonPDS.Interfaces;

namespace AxonPDS.Entities;

public class Treatment : IBaseEntity
{   
    [Required]
    public Guid Id { get; set; }

    [Required]
    public String Name { get; set; } = string.Empty;


    public String Descrption { get; set; } = string.Empty;

    [Required]
    public Boolean Status { get; set; }  = true;

    public List<TreatmentRecord> TreatmentRecords { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}
