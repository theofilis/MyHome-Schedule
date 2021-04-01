using MediatR;
using MyHome.Application.Common.Exceptions;
using MyHome.Application.Common.Services;
using MyHome.Domain.Entities;
using MyHome.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyHome.Application.Schedule.CalendarEvents.Commands.UpdateCalendarEvent
{
    public class UpdateCalendarEventCommand : IRequest<long>
    {
        public long Id { get; set; }

        public string Title { get; set; } = "";

        public bool AllDay { get; set; } = false;

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Url { get; set; } = "";

        public long? CalendarId { get; set; }

        public class UpdateGroupCommandHandler : IRequestHandler<UpdateCalendarEventCommand, long>
        {
            private readonly IApplicationDbContext _context;

            public UpdateGroupCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<long> Handle(UpdateCalendarEventCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.CalendarEvent.FindAsync(request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(CalendarEvent), request.Id);
                }

                entity.Title = request.Title;
                entity.AllDay = request.AllDay;
                entity.Start = request.Start;
                entity.End = request.End;
                entity.Url = request.Url;
                entity.CalendarId = request.CalendarId;

                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}
