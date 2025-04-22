using System;
using AxonPDS.Data;
using AxonPDS.DTOs.Treatment;
using AxonPDS.Entities;
using AxonPDS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AxonPDS.Repositories;

public class TreatmentRepo (PdsDbContext pdsDbContext) : ITreatment
{
    private readonly PdsDbContext service = pdsDbContext;

public async Task<TreatmentResponseDto> CreateTreatmentAsync(CreateTreatmentDto dto)
{
    var treatment = new Treatment
    {
        Id = Guid.NewGuid(),
        Name = dto.Name,
        Descrption = dto.Description,
        Status = dto.Active == 1,
    };
    
    service.Treatments.Add(treatment);
    await service.SaveChangesAsync();

    return new TreatmentResponseDto
    {
        Name = treatment.Name,
        Description = treatment.Descrption,
        Active = treatment.Status ? 1 : 0,
        CreatedAt = treatment.CreatedAt
    };
}

public async Task<(bool Success, string Message)> DeleteTreatmentAsync(Guid id)
{
    var isUsed = await service.TreatmentRecords
        .AnyAsync(tr => tr.TreatmentId == id);
    
    if (isUsed)
    {
        return (false, "Treatment is in use and cannot be deleted.");
    }

    var treatment = await service.Treatments.FindAsync(id);
    if (treatment == null)
    {
        return (false, "Treatment not found.");
    }

    service.Treatments.Remove(treatment);
    await service.SaveChangesAsync();
    
    return (true, "Treatment deleted successfully.");
}

    

public async Task<(List<TreatmentResponseDto>, int)> GetAllTreatmentsAsync(string? search = "", string status = "all", int page = 1, int limit = 20)
{
    var query = service.Treatments.AsQueryable();

    if (!string.IsNullOrWhiteSpace(search))
    {
        var searchWords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        query = query.Where(t => searchWords.All(word => t.Name.Contains(word) || (t.Descrption != null && t.Descrption.Contains(word))));
    }

    if (status == "active")
    {
        query = query.Where(t => t.Status);
    }
    else if (status == "inactive")
    {
        query = query.Where(t => !t.Status);
    }

    int totalTreatments = await query.CountAsync();
    var treatments = await query
        .Skip((page - 1) * limit)
        .Take(limit)
        .Select(t => new TreatmentResponseDto
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Descrption,
            Active = t.Status ? 1 : 0,
            CreatedAt = t.CreatedAt
        })
        .ToListAsync();

    return (treatments, totalTreatments);
}    

public async Task<(bool Success, string Message)> UpdateTreatmentAsync(Guid id, UpdateTreatmentDto dto)
{
    var treatment = await service.Treatments.FindAsync(id);
    if (treatment == null)
    {
        return (false, "Treatment not found.");
    }

    treatment.Name = dto.Name;
    treatment.Descrption = dto.Description;
    treatment.Status = dto.Active == 1;

    await service.SaveChangesAsync();
    return (true, "Treatment updated successfully.");
}
}
