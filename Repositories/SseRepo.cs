using System;
using System.Runtime.CompilerServices;
using AxonPDS.Data;
using Microsoft.EntityFrameworkCore;

namespace AxonPDS.Repositories;

public class SseRepo (PdsDbContext pdsDbContext)
{
     private readonly PdsDbContext service = pdsDbContext;

    private static string GetMonthName(int monthIndex)
    {
        string[] months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        return months[monthIndex - 1];
    }

    public async IAsyncEnumerable<object> StreamDataAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var currentDate = DateTime.UtcNow;
            var currentYear = currentDate.Year;
            var startOfDay = currentDate.Date;
            var startOfMonth = new DateTime(currentYear, currentDate.Month, 1);
            var startOfYear = new DateTime(currentYear, 1, 1);

            var monthlyResult = await service.Patients
                .Where(p => p.CreatedAt >= startOfYear && p.CreatedAt < new DateTime(currentYear + 1, 1, 1))
                .GroupBy(p => p.CreatedAt.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .OrderBy(g => g.Month)
                .ToListAsync(cancellationToken);

            var todayCount = await service.Patients.CountAsync(p => p.CreatedAt >= startOfDay && p.CreatedAt < startOfDay.AddDays(1), cancellationToken);
            var monthCount = await service.Patients.CountAsync(p => p.CreatedAt >= startOfMonth && p.CreatedAt < startOfMonth.AddMonths(1), cancellationToken);
            var yearCount = await service.Patients.CountAsync(p => p.CreatedAt >= startOfYear && p.CreatedAt < new DateTime(currentYear + 1, 1, 1), cancellationToken);

            var recentPatients = await service.Patients
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .Select(p => new { id = p.Id, p.Fullname, p.Email })
                .ToListAsync(cancellationToken);

            var currentMonthPatients = await service.Patients
                .Where(p => p.CreatedAt >= startOfMonth && p.CreatedAt < startOfMonth.AddMonths(1))
                .Select(p => new { 
                    id = p.Id, 
                    p.Fullname, 
                    createdAt = p.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") 
                })
                .ToListAsync(cancellationToken);

            var treatments = await service.Treatments
                .Select(t => new { id = t.Id, t.Name })
                .ToListAsync(cancellationToken);

            var medicines = await service.Medicines
                .Select(m => new { id = m.Id, m.Name })
                .ToListAsync(cancellationToken);

            yield return new
            {
                todayCount,
                monthCount,
                yearCount,
                monthlyPatientCounts = monthlyResult.Select(m => new { month = GetMonthName(m.Month), patient = m.Count }).ToList(),
                recentPatients,
                currentMonthPatients,
                treatment = treatments,
                medicine = medicines
            };

            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
        }
    }
}

