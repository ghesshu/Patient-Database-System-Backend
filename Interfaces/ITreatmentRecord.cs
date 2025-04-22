using AxonPDS.DTOs.TreatmentRecord;


namespace AxonPDS.Interfaces;

public interface ITreatmentRecord
{
    Task<(bool? Success, string Message)> UpdateTRecords(CreateTRecordDTO body);

}
