using MyHome.Domain.Common;
using MyHome.Domain.Enums;
using System.Collections.Generic;

namespace MyHome.Domain.Entities
{
    public class Calendar : Resource<string, long>
    {
        public string Title { get; set; } = "";

        public string Color { get; set; }= "";

        public string BackgroundColor { get; set; }= "";

        public CalendarSource Source { get; set; }

        public ICollection<CalendarEvent> Events { get; private set; } = new List<CalendarEvent>();
    }
}
