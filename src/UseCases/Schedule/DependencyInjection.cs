using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MyHome.Application.Schedule
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScheduleModule(this IServiceCollection services)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(currentAssembly);
            services.AddAutoMapper(currentAssembly);
            services.AddValidatorsFromAssembly(currentAssembly);
            return services;
        }
    }
}
