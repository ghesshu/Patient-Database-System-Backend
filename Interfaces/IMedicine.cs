using System;
using AxonPDS.DTOs.Medicine;
using AxonPDS.Entities;

namespace AxonPDS.Interfaces;

public interface IMedicine
{
      Task<MedicineResponseDto?> GetMedicineById(Guid id);

    Task<(List<MedicineResponseDto>, int)> GetAllMedicines(
    string? search = "",
    string status = "all",  // "all", "instock", "outofstock"
    int page = 1,
    int limit = 20
    );

    Task<Medicine?> CreateMedicine(CreateMedicineDto medicineDto);

    Task<bool> UpdateMedicine(Guid id, UpdateMedicineDto medicineDto);

    Task<bool> DeleteMedicine(Guid id);

    Task<(bool Success, string Message)> CanDeleteMedicine(Guid id);
}
