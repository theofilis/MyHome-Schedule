using Microsoft.Extensions.Logging;
using Moq;
using MyHome.Application.Common.Behaviours;
using MyHome.Application.Common.Services;
using MyHome.Application.Schedule.Calendars.Commands.CreateCalendar;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;

namespace MyHome.Application.UnitTests.Common.Behaviours
{
    public class RequestLoggerTests
    {
        private readonly Mock<ILogger<CreateCalendarCommand>> _logger;
        private readonly Mock<ICurrentUserService> _currentUserService;

        public RequestLoggerTests()
        {
            _logger = new Mock<ILogger<CreateCalendarCommand>>();

            _currentUserService = new Mock<ICurrentUserService>();
        }

        [Test]
        public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
        {
            _currentUserService.Setup(x => x.UserId).Returns("Administrator");
            var requestLogger = new LoggingBehaviour<CreateCalendarCommand>(_logger.Object, _currentUserService.Object);
            await requestLogger.Process(new CreateCalendarCommand { }, new CancellationToken());
            _currentUserService.Verify(i => i.UserId, Times.Once);
        }

        [Test]
        public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
        {
            var requestLogger = new LoggingBehaviour<CreateCalendarCommand>(_logger.Object, _currentUserService.Object);
            await requestLogger.Process(new CreateCalendarCommand { }, new CancellationToken());
            _currentUserService.Verify(i => i.UserId, Times.Exactly(2));
        }
    }
}
