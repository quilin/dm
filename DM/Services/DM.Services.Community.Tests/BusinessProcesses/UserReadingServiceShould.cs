using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Community.BusinessProcesses.Reading;
using DM.Services.Community.Dto;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Tests.Core;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Community.Tests.BusinessProcesses
{
    public class UserReadingServiceShould : UnitTestBase
    {
        private readonly ISetup<IIdentity, UserSettings> currentUserSettingsSetup;
        private readonly Mock<IUserReadingRepository> readingRepository;
        private readonly UserReadingService service;

        public UserReadingServiceShould()
        {
            var identityProvider = Mock<IIdentityProvider>();
            var identity = Mock<IIdentity>();
            identityProvider
                .Setup(p => p.Current)
                .Returns(identity.Object);
            currentUserSettingsSetup = identity.Setup(i => i.Settings);

            readingRepository = Mock<IUserReadingRepository>();

            service = new UserReadingService(identityProvider.Object, readingRepository.Object);
        }

        [Fact]
        public void ThrowGoneWhenUserNotFound()
        {
            readingRepository
                .Setup(r => r.GetUserDetails(It.IsAny<string>()))
                .ReturnsAsync((UserDetails) null);

            service.Invoking(s => s.GetDetails("User").Wait())
                .Should().Throw<HttpException>()
                .And.StatusCode.Should().Be(HttpStatusCode.Gone);
            readingRepository.Verify(r => r.GetUserDetails("User"), Times.Once);
        }

        [Fact]
        public async Task ReturnFoundUser()
        {
            var expected = new UserDetails();
            readingRepository
                .Setup(r => r.GetUserDetails(It.IsAny<string>()))
                .ReturnsAsync(expected);

            var actual = await service.GetDetails("User");

            actual.Should().Be(expected);
            readingRepository.Verify(r => r.GetUserDetails("User"), Times.Once);
        }

        [Fact]
        public async Task ReturnFetchedUsers()
        {
            var expected = new GeneralUser[0];
            readingRepository
                .Setup(r => r.CountUsers(It.IsAny<bool>()))
                .ReturnsAsync(10);
            readingRepository
                .Setup(r => r.GetUsers(It.IsAny<PagingData>(), It.IsAny<bool>()))
                .ReturnsAsync(expected);
            currentUserSettingsSetup.Returns(new UserSettings{Paging = new PagingSettings{EntitiesPerPage = 10}});

            var (actual, _) = await service.Get(new PagingQuery(), true);
            actual.Should().BeSameAs(expected);
            readingRepository.Verify(r => r.CountUsers(true), Times.Once);
            readingRepository.Verify(r => r.GetUsers(It.IsAny<PagingData>(), true));
        }
    }
}