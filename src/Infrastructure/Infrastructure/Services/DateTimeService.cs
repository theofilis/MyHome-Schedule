using MyHome.Application.Common.Services;
using System;

namespace MyHome.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
