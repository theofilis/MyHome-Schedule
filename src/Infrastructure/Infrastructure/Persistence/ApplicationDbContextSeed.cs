using MyHome.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace MyHome.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            return Task.CompletedTask;
        }
    }
}
