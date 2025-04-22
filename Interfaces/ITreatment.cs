using System;
using AxonPDS.DTOs.Treatment;
using AxonPDS.Entities;

namespace AxonPDS.Interfaces;

public interface ITreatment
{
        Task<TreatmentResponseDto> CreateTreatmentAsync(CreateTreatmentDto dto);
        Task<(bool Success, string Message)> DeleteTreatmentAsync(Guid id);
        Task<(bool Success, string Message)> UpdateTreatmentAsync(Guid id, UpdateTreatmentDto dto);
        Task<(List<TreatmentResponseDto>, int)> GetAllTreatmentsAsync
        (
            string? search = "", 
            string status = "all", 
            int page = 1, 
            int limit = 20
        );

}
