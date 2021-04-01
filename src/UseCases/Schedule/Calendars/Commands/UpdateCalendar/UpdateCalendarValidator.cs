using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MyHome.Application.Common.Services;

namespace MyHome.Application.Schedule.Calendars.Commands.UpdateCalendar
{
    public class UpdateCalendarValidator : AbstractValidator<UpdateCalendarCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCalendarValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Title)
                .NotEmpty()
                    .WithMessage("Name is required.")
                .MustAsync(BeUniqueName)
                    .WithMessage("The specified name already exists.");
        }

        public async Task<bool> BeUniqueName(UpdateCalendarCommand command,
                                             string title,
                                             CancellationToken cancellationToken)
        {
            return await _context.Calendar
                .Where(l => l.Id != command.Id)
                .AllAsync(l => l.Title != title);
        }
    }
}
