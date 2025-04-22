using Axon_Job_App.Common;
using Axon_Job_App.Data;
using Axon_Job_App.Features.Clients;
using Cai;
using JasperFx.Core.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Axon_Job_App.Features.Jobs;

[Queries]
public partial class JobQueries
{
    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Job> Jobs([Service] DataContext db) 
    {
        return db.Jobs.AsQueryable();
    }

    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<JobAssignment> JobAssignments([Service] DataContext db) 
    {
        return db.JobAssignments.AsQueryable();
    }
    
}