using System;
using AxonPDS.Data;
using AxonPDS.DTOs.Medicine;
using AxonPDS.Entities;
using AxonPDS.Interfaces;
using Microsoft.EntityFrameworkCore;
 
namespace AxonPDS.Repositories;

public class MedicineRepo (PdsDbContext pdsDbContext) : IMedicine
{
    private readonly PdsDbContext service = pdsDbContext;

    public async Task<(List<MedicineResponseDto>, int)> GetAllMedicines(
    string? search = "",
    string status = "all",  // "all", "instock", "outofstock"
    int page = 1,
    int limit = 20)
{
    var query = service.Medicines.AsQueryable();

    // Handle search (by Name or Description)
    if (!string.IsNullOrWhiteSpace(search))
    {
        var searchWords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        query = query.Where(m => searchWords.All(word => m.Name.Contains(word) || (m.Description != null && m.Description.Contains(word))));
    }

    // Handle stock status filter
    if (status == "instock")
    {
        query = query.Where(m => m.Stock > 0);
    }
    else if (status == "outofstock")
    {
        query = query.Where(m => m.Stock == 0);
    }

    // Pagination
    int totalMedicines = await query.CountAsync();
    var medicines = await query
        .Skip((page - 1) * limit)
        .Take(limit)
        .Select(m => new MedicineResponseDto
        {
            Id = m.Id,
            Name = m.Name,
            Description = m.Description,
            Stock = m.Stock,
            Measure = m.Measure,
        })
        .ToListAsync();

    return (medicines, totalMedicines);
}

   public async Task<MedicineResponseDto?> GetMedicineById(Guid id)
    {
        var medicine = await service.Medicines.FindAsync(id);

        if(medicine is null)
        {
            return null;
        }

        var newMed = new MedicineResponseDto 
        {
            Id = medicine.Id,
            Name = medicine.Name,
            Description = medicine.Description,
            Stock = medicine.Stock,
            Measure = medicine.Measure
        };

        return newMed;
    } 

    public async Task<Medicine?> CreateMedicine(CreateMedicineDto medicineDto)
    {
        var medicine = new Medicine
        {
            Id = Guid.NewGuid(),
            Name = medicineDto.Name,
            Description = medicineDto.Description,
            Stock = medicineDto.Stock,
            Measure = medicineDto.Measure
        };

        await service.Medicines.AddAsync(medicine);
        var changes = await service.SaveChangesAsync();
        
        return changes > 0 ? medicine : null;
    }

    public async Task<bool> UpdateMedicine(Guid id, UpdateMedicineDto medicineDto)
    {
        var existingMedicine = await service.Medicines.FindAsync(id);
        if (existingMedicine is null)
            return false;

        existingMedicine.Name = medicineDto.Name;
        existingMedicine.Description = medicineDto.Description ?? existingMedicine.Description;
        existingMedicine.UpdatedAt = DateTime.UtcNow;

        await service.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteMedicine(Guid id)
    {
        var medicine = await service.Medicines.FindAsync(id);
        if (medicine is null)
            return false;

        service.Medicines.Remove(medicine);
        await service.SaveChangesAsync();
        return true;
    }
    public async Task<(bool Success, string Message)> CanDeleteMedicine(Guid id)
    {
        var isUsed = await service.MedicineRecords
            .AnyAsync(mr => mr.MedicineId == id);
        
        if (isUsed)
        {
            return (false, "Medicine is in use and cannot be deleted.");
        }


        var medicine = await service.Medicines.FindAsync(id);
        if (medicine is null)
            return (false, "Medicine Cannot be found");

        service.Medicines.Remove(medicine);
        await service.SaveChangesAsync();
        return (true, "Medicine Deleted");
    }

}
