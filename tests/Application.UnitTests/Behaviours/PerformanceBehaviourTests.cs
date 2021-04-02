using Microsoft.Extensions.Logging;
using Moq;
using MyHome.Application.Common.Behaviours;
using MyHome.Application.Common.Services;
using MyHome.Application.Schedule.Calendars.Commands.CreateCalendar;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace MyHome.Application.UnitTests.Common.Behaviours
{
    public class PerformanceBehaviourTests
    {
        private readonly Mock<ILogger<CreateCalendarCommand>> _logger;
        private readonly Mock<ICurrentUserService> _currentUserService;

        public PerformanceBehaviourTests()
        {
            _logger = new Mock<ILogger<CreateCalendarCommand>>();
            _currentUserService = new Mock<ICurrentUserService>();
        }

        [Test]
        public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
        {
            _currentUserService.Setup(x => x.UserId).Returns("Administrator");
            var performanceBehaviour = new PerformanceBehaviour<CreateCalendarCommand, long>(_logger.Object, _currentUserService.Object);
            await performanceBehaviour.Handle(new CreateCalendarCommand { }, new CancellationToken(), () =>  {
                return Task.FromResult(1L); 
            });
            _currentUserService.Verify(i => i.UserId, Times.Never);
        }

        [Test]
        public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
        {
            _currentUserService.Setup(x => x.UserId).Returns("Administrator");
            var performanceBehaviour = new PerformanceBehaviour<CreateCalendarCommand, long>(_logger.Object, _currentUserService.Object);
            await performanceBehaviour.Handle(new CreateCalendarCommand { }, new CancellationToken(), () => {
                System.Threading.Thread.Sleep(1000);
                return Task.FromResult(1L);
            });
            _currentUserService.Verify(i => i.UserId, Times.Once);
        }
    }
}
