using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyHome.Domain.Entities;

namespace MyHome.Infrastructure.Persistence.Configuration
{
    #nullable disable
    public class CalendarConfiguration : IEntityTypeConfiguration<Calendar>
    {
        public void Configure(EntityTypeBuilder<Calendar> builder)
        {
            builder
                .HasMany(c => c.Events)
                    .WithOne(c => c.Calendar)
                    .HasForeignKey(c => c.CalendarId);
        }
    }
    #nullable enable
}
