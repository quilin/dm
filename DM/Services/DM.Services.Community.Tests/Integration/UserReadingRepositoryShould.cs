using System;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Reading;
using DM.Services.Core.Dto;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Tests.Core;
using FluentAssertions;
using Xunit;

namespace DM.Services.Community.Tests.Integration
{
    public class UserReadingRepositoryShould : DbTestBase
    {
        [Theory]
        [InlineData(false, 2)]
        [InlineData(true, 4)]
        public async Task CountAndSelectUsers(bool withInactive, int expected)
        {
            using var rdb = GetRdb($"{nameof(CountAndSelectUsers)}_{withInactive}");
            var rightNow = new DateTimeOffset(2019, 05, 11, 5, 2, 1, TimeSpan.Zero);
            var activeUser1 = new User
            {
                UserId = Guid.NewGuid(),
                Activated = true,
                IsRemoved = false,
                Email = "test1",
                Login = "ActiveUser1",
                LastVisitDate = rightNow.Subtract(TimeSpan.FromHours(10))
            };
            var activeUser2 = new User
            {
                UserId = Guid.NewGuid(),
                Activated = true,
                IsRemoved = false,
                Email = "test2",
                Login = "ActiveUser2",
                LastVisitDate = rightNow.Subtract(TimeSpan.FromDays(28))
            };
            var inactiveUser1 = new User
            {
                UserId = Guid.NewGuid(),
                Activated = false,
                IsRemoved = false,
                Email = "test3",
                Login = "InactiveUser1"
            };
            var inactiveUser2 = new User
            {
                UserId = Guid.NewGuid(),
                Activated = true,
                IsRemoved = false,
                Email = "test4",
                Login = "InactiveUser2",
                LastVisitDate = null
            };
            var inactiveUser3 = new User
            {
                UserId = Guid.NewGuid(),
                Activated = true,
                IsRemoved = false,
                Email = "test5",
                Login = "InactiveUser3",
                LastVisitDate = rightNow.Subtract(TimeSpan.FromDays(100))
            };
            var removedUser = new User
            {
                UserId = Guid.NewGuid(),
                Activated = true,
                IsRemoved = true,
                Email = "test6",
                Login = "RemovedUser"
            };
            rdb.Users.AddRange(activeUser1, activeUser2, inactiveUser1, inactiveUser2, inactiveUser3, removedUser);
            await rdb.SaveChangesAsync();

            var dateTimeProvider = Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(p => p.Now).Returns(rightNow);
            var readingRepository = new UserReadingRepository(rdb, null, dateTimeProvider.Object, GetMapper());

            var actualCount = await readingRepository.CountUsers(withInactive);
            actualCount.Should().Be(expected);

            var pagingData = new PagingData(new PagingQuery {Size = 1000, Skip = 0}, 1000, actualCount);
            var actualList = await readingRepository.GetUsers(pagingData, withInactive);
            actualList.Should().HaveCount(actualCount);
        }

        [Theory]
        [InlineData("someuser", true)]
        [InlineData("SOmEusER", true)]
        [InlineData("notuser", false)]
        public async Task GetUserWithMatchingLogin(string searchedLogin, bool expectedSuccess)
        {
            using var rdb = GetRdb(nameof(GetUserWithMatchingLogin));
            using var mongo = GetMongoClient(nameof(GetUserWithMatchingLogin));
            rdb.Users.Add(new User
            {
                UserId = Guid.NewGuid(),
                Activated = true,
                IsRemoved = false,
                Login = "SomeUser"
            });
            await rdb.SaveChangesAsync();

            var readingRepository = new UserReadingRepository(rdb, mongo.Client, new DateTimeProvider(), GetMapper());
            (await readingRepository.GetUserDetails(searchedLogin) != null).Should().Be(expectedSuccess);
            (await readingRepository.GetUser(searchedLogin) != null).Should().Be(expectedSuccess);
        }
    }
}