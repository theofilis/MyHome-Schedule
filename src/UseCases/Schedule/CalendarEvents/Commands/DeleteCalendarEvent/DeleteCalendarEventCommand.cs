using MediatR;
using MyHome.Application.Common.Exceptions;
using MyHome.Application.Common.Services;
using MyHome.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MyHome.Application.Schedule.CalendarEvents.Commands.DeleteCalendarEvent
{
    public class DeleteCalendarEventCommand : IRequest
    {
        public long Id { get; set; }
    }

    public class DeleteCalendarCommandHandler : IRequestHandler<DeleteCalendarEventCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCalendarCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCalendarEventCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.CalendarEvent.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(CalendarEvent), request.Id);
            }

            _context.CalendarEvent.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
