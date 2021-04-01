using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyHome.Application.Common.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MyHome.Application.Schedule.Calendars.Commands.CreateCalendar
{
    public class CreateCalendarCommandValidator : AbstractValidator<CreateCalendarCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateCalendarCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Name is required.")
                .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
        }

        public async Task<bool> BeUniqueName(CreateCalendarCommand command,
                                             string title,
                                             CancellationToken cancellationToken)
        {
            return await _context.Calendar
                .AllAsync(l => l.Title != title);
        }
    }
}
