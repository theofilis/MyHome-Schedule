using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MyHome.Application.Common.Models.Email;
using MyHome.Application.Common.Services;
using MyHome.Application.Schedule;
using MyHome.Dashboard.Configuration;
using MyHome.Dashboard.Filters;
using MyHome.Dashboard.Services;
using MyHome.Infrastructure.Configuration;
using MyHome.Infrastructure.HealthChecks;
using MyHome.Infrastructure.Persistence;
using MyHome.Infrastructure.Services;
using RazorLight;
using System.Reflection;
using System;

namespace MyHome.Dashboard
{
    public static class ServiceCollectionApiDocumentationExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
                                                           IConfiguration configuration,
                                                           IApplicationConfiguration applicationConfiguration)
        {
            // Install Infrastructure services
            services.AddPersistence(configuration);
            services.AddHealthCheck(configuration);

            // Install modules
            services.AddScheduleModule();

            var emailConfig = configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton(c => new RazorLightEngineBuilder()
              .UseEmbeddedResourcesProject(Assembly.GetExecutingAssembly())
              .UseMemoryCachingProvider()
              .Build());
            services.AddSingleton<IDateTime, DateTimeService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            return services;
        }

        public static void AddApiDocs(this IServiceCollection services, IDashboardConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(configuration.ApiConfiguration.Version, new OpenApiInfo
                {
                    Version = configuration.ApiConfiguration.Version,
                    Title = configuration.ApiConfiguration.Name,
                    Description = "",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "George Theofilis",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/theofilis"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                c.OperationFilter<AuthorizeCheckOperationFilter>();

                c.CustomSchemaIds((type) => type.FullName.Replace("MyHome.", ""));
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }
    }

    public static class ApplicationApiDocumentationExtensions
    {
        public static void UseApiDocs(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyHome v1");
            });
        }
    }
}
