using MyHome.Application.Common.Profiles;
using MyHome.Domain.Entities;
using System;

namespace MyHome.Application.Schedule.CalendarEvents.Queries.GetCalendarEvents
{
    public class CalendarEventListDto : IMapFrom<CalendarEvent>
    {
        public long? Id { get; set; }

        public string Title { get; set; } = "";

        public bool AllDay { get; set; } = false;

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Url { get; set; } = "";

        public CalendarViewDto? Calendar { get; set; }
    }
}