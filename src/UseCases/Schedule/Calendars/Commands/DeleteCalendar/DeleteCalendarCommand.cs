using MediatR;
using MyHome.Application.Common.Exceptions;
using MyHome.Application.Common.Services;
using MyHome.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MyHome.Application.Schedule.Calendars.Commands.DeleteCalendar
{
    public class DeleteCalendarCommand : IRequest
    {
        public long Id { get; set; }
    }

    public class DeleteCalendarCommandHandler : IRequestHandler<DeleteCalendarCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCalendarCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCalendarCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Calendar.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Calendar), request.Id);
            }

            _context.Calendar.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
