using System.ComponentModel.DataAnnotations;
using AxonPDS.Entities;

namespace AxonPDS.DTOs.Patient;

public record class CreatePatientDto
{
    [Required]
    public string Fullname { get; set; } = string.Empty;

    [Required, Phone]
    public string Phonenumber { get; set; } = string.Empty;

    [Required]
    public string Emergency { get; set; } = string.Empty;

    [Required]
    public Gender Gender { get; set; }

    [Required]
    public DateTime Dob { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

}
