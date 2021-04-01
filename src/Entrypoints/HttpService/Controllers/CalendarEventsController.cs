using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHome.Application.Schedule.CalendarEvents.Commands.DeleteCalendarEvent;
using MyHome.Application.Schedule.CalendarEvents.Commands.UpdateCalendarEvent;
using MyHome.Application.Schedule.CalendarEvents.Queries.GetCalendarEvent;
using MyHome.Application.Schedule.CalendarEvents.Queries.GetCalendarEvents;
using MyHome.Application.Schedule.CalendarEvents.Commands.CreateCalendarEvent;
using MyHome.Dashboard.Filters;
using MyHome.Dashboard.ViewModels;
using System.Threading.Tasks;
using MyHome.Application.Common.Models.Pagination;

namespace MyHome.Dashboard.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarEventsController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<long>> Create(CreateCalendarEventCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateCalendarEventCommand command)
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
        public async Task<PageResult<CalendarEventListDto>> Get([FromQuery] SmartTableQueryParameters parameters)
        {
            return await Mediator.Send(new GetCalendarEventsQuery { Query = parameters });
        }

        [HttpGet("{id:long}", Name = "GetCalendarEvent")]
        public async Task<CalendarEventViewDto> Get(long id)
        {
            return await Mediator.Send(new GetCalendarEventQuery { Id = id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await Mediator.Send(new DeleteCalendarEventCommand { Id = id });

            return NoContent();
        }
    }
}
