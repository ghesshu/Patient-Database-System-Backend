using System;
using AxonPDS.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AxonPDS.Entities;

public class User : IdentityUser<Guid>, IBaseEntity
{
    public String FullName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
