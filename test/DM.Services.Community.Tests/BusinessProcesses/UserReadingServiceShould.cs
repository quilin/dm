using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Community.BusinessProcesses.Users.Reading;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Tests.Core;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Community.Tests.BusinessProcesses;

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
    public async Task ThrowGoneWhenUserDetailsNotFound()
    {
        readingRepository
            .Setup(r => r.GetUserDetails(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserDetails) null);

        (await service.Invoking(s => s.GetDetails("User", CancellationToken.None))
            .Should().ThrowAsync<HttpException>())
            .And.StatusCode.Should().Be(HttpStatusCode.Gone);
        readingRepository.Verify(r => r.GetUserDetails("User", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReturnFoundUserDetails()
    {
        var expected = new UserDetails();
        readingRepository
            .Setup(r => r.GetUserDetails(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var actual = await service.GetDetails("User", CancellationToken.None);

        actual.Should().Be(expected);
        readingRepository.Verify(r => r.GetUserDetails("User", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ThrowGoneWhenUserNotFound()
    {
        readingRepository
            .Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GeneralUser) null);

        (await service.Invoking(s => s.Get("User", CancellationToken.None))
            .Should().ThrowAsync<HttpException>())
            .And.StatusCode.Should().Be(HttpStatusCode.Gone);
        readingRepository.Verify(r => r.GetUser("User", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReturnFoundUser()
    {
        var expected = new GeneralUser();
        readingRepository
            .Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var actual = await service.Get("User", CancellationToken.None);

        actual.Should().Be(expected);
        readingRepository.Verify(r => r.GetUser("User", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReturnFetchedUsers()
    {
        var expected = Array.Empty<GeneralUser>();
        readingRepository
            .Setup(r => r.CountUsers(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(10);
        readingRepository
            .Setup(r => r.GetUsers(It.IsAny<PagingData>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        currentUserSettingsSetup.Returns(new UserSettings{Paging = new PagingSettings{EntitiesPerPage = 10}});

        var (actual, _) = await service.Get(new PagingQuery(), true, CancellationToken.None);
        actual.Should().BeSameAs(expected);
        readingRepository.Verify(r => r.CountUsers(true, It.IsAny<CancellationToken>()), Times.Once);
        readingRepository.Verify(r => r.GetUsers(It.IsAny<PagingData>(), true, It.IsAny<CancellationToken>()));
    }
}