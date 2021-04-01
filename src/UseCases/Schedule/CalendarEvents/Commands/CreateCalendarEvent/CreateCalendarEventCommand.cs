using MediatR;
using MyHome.Application.Common.Services;
using MyHome.Domain.Entities;
using MyHome.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyHome.Application.Schedule.CalendarEvents.Commands.CreateCalendarEvent
{
    public class CreateCalendarEventCommand : IRequest<long>
    {
        public string Title { get; set; } = "";

        public bool AllDay { get; set; } = false;

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Url { get; set; } = "";

        public long? CalendarId { get; set; }

        public class CreateCalendarCommandHandler : IRequestHandler<CreateCalendarEventCommand, long>
        {
            private readonly IApplicationDbContext _context;

            public CreateCalendarCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<long> Handle(CreateCalendarEventCommand request, CancellationToken cancellationToken)
            {
                var entity = new CalendarEvent
                {
                    Title = request.Title,
                    AllDay = request.AllDay,
                    Start = request.Start,
                    End = request.End,
                    Url = request.Url,
                    CalendarId = request.CalendarId,
                };

                _context.CalendarEvent.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}
