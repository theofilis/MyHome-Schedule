using MediatR;
using MyHome.Application.Common.Services;
using MyHome.Domain.Entities;
using MyHome.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace MyHome.Application.Schedule.Calendars.Commands.CreateCalendar
{
    public class CreateCalendarCommand : IRequest<long>
    {
        public string Title { get; set; } = "";

        public string Color { get; set; } = "";

        public string BackgroundColor { get; set; } = "";

        public class CreateCalendarCommandHandler : IRequestHandler<CreateCalendarCommand, long>
        {
            private readonly IApplicationDbContext _context;

            public CreateCalendarCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<long> Handle(CreateCalendarCommand request, CancellationToken cancellationToken)
            {
                var entity = new Calendar
                {
                    Title = request.Title,
                    Color = request.Color,
                    BackgroundColor = request.BackgroundColor,
                    Source = CalendarSource.MyHome,
                };

                _context.Calendar.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}
