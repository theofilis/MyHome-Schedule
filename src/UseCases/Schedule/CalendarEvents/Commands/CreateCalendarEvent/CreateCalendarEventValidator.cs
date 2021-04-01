using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyHome.Application.Common.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MyHome.Application.Schedule.CalendarEvents.Commands.CreateCalendarEvent
{
    public class CreateCalendarCommandValidator : AbstractValidator<CreateCalendarEventCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateCalendarCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Name is required.")
                .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
        }

        public async Task<bool> BeUniqueName(CreateCalendarEventCommand command,
                                             string title,
                                             CancellationToken cancellationToken)
        {
            return await _context.CalendarEvent
                .AllAsync(l => l.Title != title);
        }
    }
}
