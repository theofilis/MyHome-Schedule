using Microsoft.EntityFrameworkCore;
using MyHome.Domain.Common;
using MyHome.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MyHome.Application.Common.Services
{
    public interface IApplicationDbContext
    {        
        DbSet<Calendar> Calendar { get; set; }

        DbSet<CalendarEvent> CalendarEvent { get; set; }
     
        Task<AuditableEntity<string>> Get<EntityKey> (string identifier, EntityKey key);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
