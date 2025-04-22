using System;

namespace AxonPDS.Interfaces;

public interface IBaseEntity
{
    DateTime CreatedAt { get; set; }
    
    DateTime UpdatedAt { get; set; }

}
