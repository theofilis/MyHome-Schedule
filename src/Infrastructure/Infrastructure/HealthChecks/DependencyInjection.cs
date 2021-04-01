using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace MyHome.Infrastructure.HealthChecks
{
    public static class DependencyInjection
    {
        public static void AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecksUI(setup => setup.DisableDatabaseMigrations())
                .AddInMemoryStorage();
            services
                .AddHealthChecks()
                .AddNpgSql(configuration.GetConnectionString("DefaultConnection"))
                // .AddDiskStorageHealthCheck(setup =>
                // {
                //     setup.AddDrive("D:\\", 104857600);
                // }, tags: new string[] { "diskstorage" })
                .AddPrivateMemoryHealthCheck(GetMaximunMemory(), tags: new string[] { "privatememory" });
        }

        private static long GetMaximunMemory() =>
            Process.GetCurrentProcess().PrivateMemorySize64 + 104857600;
    }

    public static class ApplicationHealthCheckExtensions
    {
        public static void UseHealthCheck(this IApplicationBuilder app, string customStyle)
        {
            app
                .UseRouting()
                .UseEndpoints(config =>
                {
                    config.MapHealthChecks("healthz", new HealthCheckOptions()
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });
                    config.MapHealthChecksUI(setup =>
                    {
                        if (!string.IsNullOrEmpty(customStyle))
                            setup.AddCustomStylesheet(customStyle);
                    });
                });
        }
    }
}
