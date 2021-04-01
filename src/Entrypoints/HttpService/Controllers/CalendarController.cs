using Microsoft.AspNetCore.Mvc;
using MyHome.Application.Schedule.Calendars.Commands.DeleteCalendar;
using MyHome.Application.Schedule.Calendars.Commands.UpdateCalendar;
using MyHome.Application.Schedule.Calendars.Queries.GetCalendar;
using MyHome.Application.Schedule.Calendars.Queries.GetCalendars;
using MyHome.Application.Schedule.Calendars.Commands.CreateCalendar;
using MyHome.Application.Schedule.Calendars.Queries.GetCalendarEvents;
using MyHome.Dashboard.Filters;
using MyHome.Dashboard.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using MyHome.Application.Common.Models.Pagination;

namespace MyHome.Dashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<long>> Create(CreateCalendarCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateCalendarCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpGet]
        [Pagination]
        public async Task<PageResult<CalendarListDto>> Get([FromQuery] SmartTableQueryParameters parameters)
        {
            return await Mediator.Send(new GetCalendarsQuery { Query = parameters });
        }

        [HttpGet("{id:long}", Name = "GetCalendar")]
        public async Task<CalendarViewDto> Get(long id)
        {
            return await Mediator.Send(new GetCalendarQuery { Id = id });
        }

        [HttpGet("{id:long}/Events", Name = "GetCalendarEvents")]
        public async Task<List<CalendarEventListDto>> GetCalendarEvents(long id)
        {
            return await Mediator.Send(new GetCalendarEventsQuery { CalendarId = id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await Mediator.Send(new DeleteCalendarCommand { Id = id });

            return NoContent();
        }
    }
}
