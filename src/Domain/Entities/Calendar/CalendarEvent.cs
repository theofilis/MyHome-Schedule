using MyHome.Domain.Common;
using System;

namespace MyHome.Domain.Entities
{
    public class CalendarEvent : Resource<string, long>
    {
        public string Title { get; set; } = "";

        public bool AllDay { get; set; } = false;

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Url { get; set; } = "";

        public long? CalendarId { get; set; }

        public Calendar? Calendar { get; set; }
    }
}
