using MyHome.Application.Common.Profiles;
using MyHome.Domain.Entities;
using MyHome.Domain.Enums;

namespace MyHome.Application.Schedule.CalendarEvents.Queries.GetCalendarEvents
{
    public class CalendarViewDto : IMapFrom<Calendar>
    {
        public long Id { get; set; }

        public string Title { get; set; } = "";

        public string Color { get; set; } = "";

        public string BackgroundColor { get; set; } = "";

        public CalendarSource Source { get; set; }
    }
}