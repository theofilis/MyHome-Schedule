using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyHome.Dashboard.Configuration;
using MyHome.Infrastructure.HealthChecks;
using Newtonsoft.Json;

namespace MyHome.Dashboard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var dashboardConfiguration = CreateDashboardConfiguration();
            services.AddSingleton(dashboardConfiguration);

            services.AddInfrastructure(Configuration, dashboardConfiguration);
            services.AddHttpContextAccessor();
            services.AddApiDocs(dashboardConfiguration);

            services
                 .AddControllersWithViews()
                 .AddNewtonsoftJson(options =>
                 {
                     options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                     options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Populate;
                     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                     options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                     options.UseMemberCasing();
                 });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseHealthCheck("dotnet.css");
            app.UseApiDocs();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }

        protected IDashboardConfiguration CreateDashboardConfiguration()
        {
            var dashboardConfiguration = new DashboardConfiguration
            {
                ApiConfiguration = Configuration
                    .GetSection(ConfigurationConsts.ApiConfiguration)
                    .Get<ApiConfiguration>(),
            };
            return dashboardConfiguration;
        }
    }
}
