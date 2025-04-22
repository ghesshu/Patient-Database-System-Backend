using Axon_Job_App.Common;
using Axon_Job_App.Data;
using Cai;
using Microsoft.EntityFrameworkCore;

namespace Axon_Job_App.Features.Clients;

[Queries]
public partial class ClientQueries
{
    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Client> Clients([Service] DataContext db) 
    {
        return db.Clients.AsQueryable();
    }

}