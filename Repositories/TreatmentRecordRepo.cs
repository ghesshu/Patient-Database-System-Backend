using AxonPDS.Data;
using AxonPDS.DTOs.TreatmentRecord;
using AxonPDS.Entities;
using AxonPDS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AxonPDS.Repositories;

public class TreatmentRecordRepo(PdsDbContext pdsDbContext) : ITreatmentRecord
{
    private readonly PdsDbContext service = pdsDbContext;

    public async Task<(bool? Success, string Message)> UpdateTRecords(CreateTRecordDTO body)
    {
        var treatmentData = await service.TreatmentRecords
            .Where(tr => tr.RecordId == body.RecordId)
            .ToListAsync();

        // Find records to delete (exists in DB but not in request body)
        var toDelete = treatmentData
            .Where(tr => !body.TreatmentIds.Contains(tr.TreatmentId))
            .ToList();

        // Find new records to add (exists in request body but not in DB)
        var toAdd = body.TreatmentIds
            .Where(id => !treatmentData.Any(tr => tr.TreatmentId == id))
            .Select(id => new TreatmentRecord
            {
                RecordId = body.RecordId,
                TreatmentId = id
            })
            .ToList();

        // Apply changes
        if (toDelete.Count != 0)
        {
            service.TreatmentRecords.RemoveRange(toDelete);
        }

        if (toAdd.Count != 0)
        {
            await service.TreatmentRecords.AddRangeAsync(toAdd);
        }

        await service.SaveChangesAsync();

        return (true, "Treatments updated");
    }

}
