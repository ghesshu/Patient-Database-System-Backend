using System;
using AxonPDS.DTOs.Record;
using AxonPDS.Entities;

namespace AxonPDS.Interfaces;

public interface IRecord
{
    Task<(bool Success, string Message, RecordResponseDto? Record)> CreateRecordAsync(CreateRecordDto dto);

    Task<(bool Success, string Message, RecordResponseDto? Record)> UpdateRecordAsync(Guid id, CreateRecordDto dto);

    Task<(bool Success, string Message)> DeleteRecordAsync(Guid id);

    Task<(bool Success, string Message, List<RecordResponseDto?> Records)> GetRecordsByPatientIdAsync(Guid patientId);

}
