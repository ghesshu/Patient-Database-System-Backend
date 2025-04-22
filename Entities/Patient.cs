using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AxonPDS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AxonPDS.Entities;

public enum Gender
{
    Male, 
    Female,
    Other
}

public class Patient : IBaseEntity
{
    public Guid Id { get; set; }

    [Required]
    public string Fullname { get; set; } = string.Empty;

    [Required]
    public string Phonenumber { get; set; } = string.Empty;

    [Required]
    public string Emergency { get; set; } = string.Empty;

    [Required]
    public Gender Gender { get; set; }

    [Required]
    public DateTime Dob { get; set; }  

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty; 

    [JsonIgnore] // Prevents infinite loop
    public List<Record> Records { get; set; } = [];

    [JsonIgnore] // Prevents infinite loop
    public Guid? PatientInfoId { get; set; }

    public PatientInfo? PatientInfo { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
