using System;
using AxonPDS.DTOs.MedicineRecord;
using AxonPDS.Entities;

namespace AxonPDS.Interfaces;

public interface IMedicineRecord
{
    Task<(bool, string)> CreateMRecord(CreateMedicineRecordDto medicineRecord);

    Task<(bool, string)>  DeleteMRecord(Guid id);
}