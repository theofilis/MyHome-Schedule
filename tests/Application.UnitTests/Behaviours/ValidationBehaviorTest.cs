using Microsoft.Extensions.Logging;
using Moq;
using MyHome.Application.Common.Behaviours;
using MyHome.Application.Schedule.Calendars.Commands.CreateCalendar;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using System.Collections.Generic;

namespace MyHome.Application.UnitTests.Common.Behaviours
{
    class CreateCalendarCommandValidator : AbstractValidator<CreateCalendarCommand>
    {
        public CreateCalendarCommandValidator()
        {
            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Name is required.");
        }
    }

    class CreateSecondCalendarCommandValidator : AbstractValidator<CreateCalendarCommand>
    {
        public CreateSecondCalendarCommandValidator()
        {
            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Name is required.");
        }
    }

    public class ValidationBehaviorTest
    {
        private readonly Mock<ILogger<CreateCalendarCommand>> _logger;

        public ValidationBehaviorTest()
        {
            _logger = new Mock<ILogger<CreateCalendarCommand>>();
        }

        [Test]
        public Task ShouldRaiseValidationException()
        {
            var validator = new List<IValidator<CreateCalendarCommand>>
            {
                new CreateCalendarCommandValidator(),
            };

            var requestLogger = new ValidationBehavior<CreateCalendarCommand, long>(validator);

            Assert.CatchAsync<Application.Common.Exceptions.ValidationException>(async () => {
                await requestLogger.Handle(new CreateCalendarCommand { }, new CancellationToken(), () => {
                    return Task.FromResult(1L);
                });            
            });

            return Task.CompletedTask;
        }

        [Test]
        public async Task ShouldNotRaiseValidationException()
        {
            var validator = new List<IValidator<CreateCalendarCommand>>
            {
                new CreateCalendarCommandValidator(),
            };

            var requestLogger = new ValidationBehavior<CreateCalendarCommand, long>(validator);

            await requestLogger.Handle(new CreateCalendarCommand { Title = "Test" }, new CancellationToken(), () => {
                return Task.FromResult(1L);
            });
        }

        [Test]
        public Task ShouldRaiseValidationExceptionWithMultipleValidation()
        {
            var validator = new List<IValidator<CreateCalendarCommand>>
            {
                new CreateCalendarCommandValidator(),
                new CreateSecondCalendarCommandValidator(),
            };

            var requestLogger = new ValidationBehavior<CreateCalendarCommand, long>(validator);

            Assert.CatchAsync<Application.Common.Exceptions.ValidationException>(async () => {
                await requestLogger.Handle(new CreateCalendarCommand { }, new CancellationToken(), () => {
                    return Task.FromResult(1L);
                });
            });

            return Task.CompletedTask;
        }

        [Test]
        public async Task ShouldNotRaiseValidationExceptionWithMultipleValidation()
        {
            var validator = new List<IValidator<CreateCalendarCommand>>
            {
                new CreateCalendarCommandValidator(),
                 new CreateSecondCalendarCommandValidator(),
            };

            var requestLogger = new ValidationBehavior<CreateCalendarCommand, long>(validator);

            await requestLogger.Handle(new CreateCalendarCommand { Title = "Test" }, new CancellationToken(), () => {
                return Task.FromResult(1L);
            });
        }
    }
}
