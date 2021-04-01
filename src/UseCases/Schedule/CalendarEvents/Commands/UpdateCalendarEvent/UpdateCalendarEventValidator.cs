using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MyHome.Application.Common.Services;

namespace MyHome.Application.Schedule.CalendarEvents.Commands.UpdateCalendarEvent
{
    public class UpdateCalendarEventValidator : AbstractValidator<UpdateCalendarEventCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCalendarEventValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Title)
                .NotEmpty()
                    .WithMessage("Name is required.")
                .MustAsync(BeUniqueName)
                    .WithMessage("The specified name already exists.");
        }

        public async Task<bool> BeUniqueName(UpdateCalendarEventCommand command,
                                             string title,
                                             CancellationToken cancellationToken)
        {
            return await _context.CalendarEvent
                .Where(l => l.Id != command.Id)
                .AllAsync(l => l.Title != title);
        }
    }
}
