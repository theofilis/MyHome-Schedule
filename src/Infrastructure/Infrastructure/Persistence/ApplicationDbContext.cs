using Microsoft.EntityFrameworkCore;
using MyHome.Application.Common.Services;
using MyHome.Domain.Common;
using MyHome.Domain.Entities;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MyHome.Infrastructure.Persistence
{
    #nullable disable
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private ICurrentUserService _currentUserService;

        private IDateTime _dateTime;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options)
        {
            _dateTime = dateTime;
            _currentUserService = currentUserService;
        }

        public DbSet<Calendar> Calendar { get; set; }

        public DbSet<CalendarEvent> CalendarEvent { get; set; }

        public Task<AuditableEntity<string>> Get<EntityKey> (string identifier, EntityKey key) {
            throw new System.Exception($"Collection with name {identifier} not found");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity<string>>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.OwnerId = _currentUserService.UserId;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
    #nullable enable
}