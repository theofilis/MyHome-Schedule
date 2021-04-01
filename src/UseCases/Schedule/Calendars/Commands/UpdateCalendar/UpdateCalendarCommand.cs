using MediatR;
using MyHome.Application.Common.Exceptions;
using MyHome.Application.Common.Services;
using MyHome.Domain.Entities;
using MyHome.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace MyHome.Application.Schedule.Calendars.Commands.UpdateCalendar
{
    public class UpdateCalendarCommand : IRequest<long>
    {
        public long Id { get; set; }

        public string Title { get; set; } = "";

        public string Color { get; set; } = "";

        public string BackgroundColor { get; set; } = "";

        public CalendarSource Source { get; set; }

        public class UpdateGroupCommandHandler : IRequestHandler<UpdateCalendarCommand, long>
        {
            private readonly IApplicationDbContext _context;

            public UpdateGroupCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<long> Handle(UpdateCalendarCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.Calendar.FindAsync(request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Calendar), request.Id);
                }

                entity.BackgroundColor = request.BackgroundColor;
                entity.Color = request.Color;
                entity.Title = request.Title;
                entity.Source = request.Source;

                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}
