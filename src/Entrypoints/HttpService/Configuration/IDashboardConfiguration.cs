using MyHome.Infrastructure.Configuration;

namespace MyHome.Dashboard.Configuration
{
    public interface IDashboardConfiguration : IApplicationConfiguration
    {
        IApiConfiguration ApiConfiguration { get; set; }
    }
}