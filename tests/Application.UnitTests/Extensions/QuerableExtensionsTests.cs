using Microsoft.Extensions.Logging;
using Moq;
using MyHome.Application.Common.Services;
using MyHome.Application.Common.Extensions;
using MyHome.Application.Common.Models.Pagination;
using MyHome.Application.Schedule.Calendars.Commands.CreateCalendar;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MyHome.Common.UnitTests.Extensions
{
    internal class Item
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }

    public class QuerableExtensionsTests
    {
        private readonly Mock<ILogger<CreateCalendarCommand>> _logger;
        private readonly Mock<ICurrentUserService> _currentUserService;

        public QuerableExtensionsTests()
        {
            _logger = new Mock<ILogger<CreateCalendarCommand>>();
            _currentUserService = new Mock<ICurrentUserService>();
        }

        [Test]
        public void ShouldHaveOnePages()
        {
            // Create an array of Pets.
            var items = new Item[] {
                new Item { Name="Barley", Age=10 },
                new Item { Name="Boots", Age=4 },
                new Item { Name="Whiskers", Age=6 }
            }.AsQueryable();

            var pagedResult = items.ToPageList(
                new QueryParameters { 
                    Limit = 10,
                    Page = 1,
                });

            Assert.AreEqual(1, pagedResult.CurrentPage);
            Assert.AreEqual(1, pagedResult.TotalPages);
            Assert.AreEqual(3, pagedResult.TotalCount);
            Assert.IsFalse(pagedResult.HasPrevious);
            Assert.IsFalse(pagedResult.HasNext);
        }

        [Test]
        public void ShouldHaveTenPages()
        {
            // Create an array of Pets.
            var items = new List<Item>();

            for (var i = 0; i < 100; i++)
            {
                items.Add(new Item { Name = $"Name{i}", Age = 1 });
            }

            var pagedResult = items.AsQueryable().ToPageList(
                new QueryParameters
                {
                    Limit = 10,
                    Page = 1,
                });

            Assert.AreEqual(1, pagedResult.CurrentPage);
            Assert.AreEqual(10, pagedResult.TotalPages);
            Assert.AreEqual(100, pagedResult.TotalCount);
            Assert.IsFalse(pagedResult.HasPrevious);
            Assert.IsTrue(pagedResult.HasNext);
        }

        [Test]
        public void ShouldByOrdyByName()
        {
            var items = new List<Item>();

            for (var i = 0; i < 100; i++)
            {
                items.Add(new Item { Name = $"Name{100 - i}", Age = 1 });
            }

            var pagedResult = items.AsQueryable().ApplySort(
                new QueryParameters
                {
                    Limit = 10,
                    Page = 1,
                    SortDir = "ASC",
                    SortField = "Name",
                });

            Assert.AreEqual("Name1", pagedResult.First().Name);
            Assert.AreEqual(1, pagedResult.First().Age);
        }

        [Test]
        public void ShouldByOrdyByNameDescending()
        {
            // Create an array of Pets.
            var items = new Item[] {
                new Item { Name="Barley", Age=10 },
                new Item { Name="Boots", Age=4 },
                new Item { Name="Whiskers", Age=6 }
            }.AsQueryable();

            var pagedResult = items.ApplySort(
                new QueryParameters
                {
                    Limit = 10,
                    Page = 1,
                    SortDir = "DESC",
                    SortField = "Name",
                });

            Assert.AreEqual("Whiskers", pagedResult.First().Name);
            Assert.AreEqual(6, pagedResult.First().Age);
        }

    }
}
