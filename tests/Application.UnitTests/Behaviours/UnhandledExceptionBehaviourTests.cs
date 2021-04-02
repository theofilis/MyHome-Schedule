using Microsoft.Extensions.Logging;
using Moq;
using MyHome.Application.Common.Behaviours;
using MyHome.Application.Schedule.Calendars.Commands.CreateCalendar;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace MyHome.Application.UnitTests.Common.Behaviours
{
    public class UnhandledExceptionBehaviourTests
    {
        private readonly Mock<ILogger<CreateCalendarCommand>> _logger;

        public UnhandledExceptionBehaviourTests()
        {
            _logger = new Mock<ILogger<CreateCalendarCommand>>();
        }

        [Test]
        public async Task ShouldCallGetUserNameAsyncNeverIfAuthenticated()
        {
            var performanceBehaviour = new UnhandledExceptionBehaviour<CreateCalendarCommand, long>(_logger.Object);
            await performanceBehaviour.Handle(new CreateCalendarCommand { }, new CancellationToken(), () =>
            {
                return Task.FromResult(1L);
            });
            _logger.Verify(i => i.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Never);
        }

        [Test]
        public void ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
        {
            var performanceBehaviour = new UnhandledExceptionBehaviour<CreateCalendarCommand, long>(_logger.Object);

            Assert.CatchAsync<Exception>(async () =>
            {
                await performanceBehaviour.Handle(new CreateCalendarCommand { }, new CancellationToken(), () =>
                {
                    throw new Exception("Unhandled exception");
                });
            });
            _logger.Verify(i => i.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }
    }
}
