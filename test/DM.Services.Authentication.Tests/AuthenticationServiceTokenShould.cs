using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Factories;
using DM.Services.Authentication.Implementation;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Authentication.Repositories;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using DM.Tests.Core;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Xunit;
using Session = DM.Services.Authentication.Dto.Session;

namespace DM.Services.Authentication.Tests;

public class AuthenticationServiceTokenShould : UnitTestBase
{
    private readonly AuthenticationService service;
    private readonly Mock<IAuthenticationRepository> authenticationRepository;
    private readonly Mock<IDateTimeProvider> dateTimeProvider;
    private readonly ISetup<ISymmetricCryptoService, Task<string>> tokenDecryptSetup;
    private readonly Mock<ISymmetricCryptoService> cryptoService;
    private readonly Mock<IUpdateBuilder<User>> updateBuilder;

    public AuthenticationServiceTokenShould()
    {
        var securityManager = Mock<ISecurityManager>();
        cryptoService = Mock<ISymmetricCryptoService>();
        tokenDecryptSetup = cryptoService.Setup(s => s.Decrypt(It.IsAny<string>()));

        authenticationRepository = Mock<IAuthenticationRepository>();

        var sessionFactory = Mock<ISessionFactory>();

        dateTimeProvider = Mock<IDateTimeProvider>();

        var identityProvider = Mock<IIdentityProvider>();

        updateBuilder = Mock<IUpdateBuilder<User>>();
        var updateBuilderFactory = Mock<IUpdateBuilderFactory>();
        updateBuilderFactory
            .Setup(f => f.Create<User>(It.IsAny<Guid>()))
            .Returns(updateBuilder.Object);

        service = new AuthenticationService(securityManager.Object, cryptoService.Object,
            authenticationRepository.Object, sessionFactory.Object, dateTimeProvider.Object,
            identityProvider.Object, updateBuilderFactory.Object);
    }

    [Fact]
    public async Task FailIfErrorOnDecryptingToken()
    {
        tokenDecryptSetup.ThrowsAsync(new Exception());
        var actual = await service.Authenticate("token");
        actual.Error.Should().Be(AuthenticationError.ForgedToken);
        cryptoService.Verify(s => s.Decrypt("token"));
    }

    [Fact]
    public async Task FailIfErrorOnDeserializingToken()
    {
        tokenDecryptSetup.ReturnsAsync("some invalid decrypted string");
        var actual = await service.Authenticate("token");
        actual.Error.Should().Be(AuthenticationError.ForgedToken);
        cryptoService.Verify(s => s.Decrypt("token"));
    }

    [Fact]
    public async Task FailIfDeserializedTokenIsInvalid()
    {
        tokenDecryptSetup.ReturnsAsync("{\"something\": \"invalid\"}");
        var actual = await service.Authenticate("token");
        actual.Error.Should().Be(AuthenticationError.ForgedToken);
        cryptoService.Verify(s => s.Decrypt("token"));
    }

    [Fact]
    public async Task FailWhenNonPersistentSessionExpiredAndRemoveExpiredSession()
    {
        var userId = Guid.Parse("7932d1d9-0a1a-4e16-b53f-5c213a2fc097");
        var sessionId = Guid.Parse("6f9e570c-1dca-4cca-be93-d7418b85959e");
        tokenDecryptSetup.ReturnsAsync(
            "{\"userId\": \"7932d1d9-0a1a-4e16-b53f-5c213a2fc097\"," +
            " \"sessionId\": \"6f9e570c-1dca-4cca-be93-d7418b85959e\"}");

        authenticationRepository
            .Setup(r => r.FindUser(It.IsAny<Guid>()))
            .ReturnsAsync(new AuthenticatedUser());
        authenticationRepository
            .Setup(r => r.FindUserSettings(It.IsAny<Guid>()))
            .ReturnsAsync(new UserSettings());
        authenticationRepository
            .Setup(r => r.FindUserSession(It.IsAny<Guid>()))
            .ReturnsAsync(new Session
            {
                Persistent = false,
                ExpirationDate = new DateTime(2018, 06, 11, 9, 59, 59)
            });
        authenticationRepository
            .Setup(r => r.RemoveSession(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);
        dateTimeProvider
            .Setup(p => p.Now)
            .Returns(new DateTime(2018, 06, 11, 10, 0, 0));

        var actual = await service.Authenticate("token");
        actual.Error.Should().Be(AuthenticationError.SessionExpired);
        cryptoService.Verify(s => s.Decrypt("token"));
        authenticationRepository.Verify(r => r.FindUser(userId), Times.Once);
        authenticationRepository.Verify(r => r.FindUserSettings(userId), Times.Once);
        authenticationRepository.Verify(r => r.FindUserSession(sessionId), Times.Once);
        authenticationRepository.Verify(r => r.RemoveSession(userId, sessionId), Times.Once);
        authenticationRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task FailWhenInvalidSessionFound()
    {
        tokenDecryptSetup.ReturnsAsync(
            "{\"userId\": \"7932d1d9-0a1a-4e16-b53f-5c213a2fc097\"," +
            " \"sessionId\": \"6f9e570c-1dca-4cca-be93-d7418b85959e\"}");
            
        authenticationRepository
            .Setup(r => r.FindUser(It.IsAny<Guid>()))
            .ReturnsAsync(new AuthenticatedUser());
        authenticationRepository
            .Setup(r => r.FindUserSettings(It.IsAny<Guid>()))
            .ReturnsAsync(new UserSettings());
        authenticationRepository
            .Setup(r => r.FindUserSession(It.IsAny<Guid>()))
            .ReturnsAsync((Session) null);
            
        var actual = await service.Authenticate("token");
        actual.Error.Should().Be(AuthenticationError.SessionExpired);
    }

    [Fact]
    public async Task SucceedAndRefreshSessionWhenNonPersistentSessionAboutToExpire()
    {
        var userId = Guid.Parse("7932d1d9-0a1a-4e16-b53f-5c213a2fc097");
        var sessionId = Guid.Parse("6f9e570c-1dca-4cca-be93-d7418b85959e");
        tokenDecryptSetup.ReturnsAsync(
            "{\"userId\": \"7932d1d9-0a1a-4e16-b53f-5c213a2fc097\"," +
            " \"sessionId\": \"6f9e570c-1dca-4cca-be93-d7418b85959e\"}");

        var user = new AuthenticatedUser();
        var settings = new UserSettings();
        var session = new Session
        {
            Id = sessionId,
            Persistent = false,
            ExpirationDate = new DateTime(2018, 06, 11, 10, 0, 0)
        };
        authenticationRepository
            .Setup(r => r.FindUser(It.IsAny<Guid>()))
            .ReturnsAsync(user);
        authenticationRepository
            .Setup(r => r.FindUserSettings(It.IsAny<Guid>()))
            .ReturnsAsync(settings);
        authenticationRepository
            .Setup(r => r.FindUserSession(It.IsAny<Guid>()))
            .ReturnsAsync(session);
        authenticationRepository
            .Setup(r => r.RefreshSession(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateTimeOffset>()))
            .Returns(Task.CompletedTask);
        dateTimeProvider
            .Setup(p => p.Now)
            .Returns(new DateTime(2018, 06, 11, 9, 50, 0));
        updateBuilder
            .Setup(b => b.Field(u => u.LastVisitDate, It.IsAny<DateTimeOffset>()))
            .Returns(updateBuilder.Object);

        var actual = await service.Authenticate("token");
        actual.Error.Should().Be(AuthenticationError.NoError);
        actual.User.Should().Be(user);
        actual.Session.Should().Be(session);
        actual.Settings.Should().Be(settings);
        actual.AuthenticationToken.Should().Be("token");

        cryptoService.Verify(s => s.Decrypt("token"));
        authenticationRepository.Verify(r => r.FindUser(userId), Times.Once);
        authenticationRepository.Verify(r => r.FindUserSettings(userId), Times.Once);
        authenticationRepository.Verify(r => r.FindUserSession(sessionId), Times.Once);
        authenticationRepository.Verify(r => r.RefreshSession(userId, sessionId, new DateTimeOffset(new DateTime(2018, 06, 11, 10, 20, 0))));
        authenticationRepository.Verify(r => r.UpdateActivity(updateBuilder.Object), Times.Once);
        authenticationRepository.VerifyNoOtherCalls();

        updateBuilder.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SucceedWhenSessionNotYetExpired()
    {
        var userId = Guid.Parse("7932d1d9-0a1a-4e16-b53f-5c213a2fc097");
        var sessionId = Guid.Parse("6f9e570c-1dca-4cca-be93-d7418b85959e");
        tokenDecryptSetup.ReturnsAsync(
            "{\"userId\": \"7932d1d9-0a1a-4e16-b53f-5c213a2fc097\"," +
            " \"sessionId\": \"6f9e570c-1dca-4cca-be93-d7418b85959e\"}");

        var user = new AuthenticatedUser();
        var settings = new UserSettings();
        var session = new Session
        {
            Id = sessionId,
            Persistent = false,
            ExpirationDate = new DateTimeOffset(new DateTime(2019, 06, 11, 10, 0, 0))
        };
        authenticationRepository
            .Setup(r => r.FindUser(It.IsAny<Guid>()))
            .ReturnsAsync(user);
        authenticationRepository
            .Setup(r => r.FindUserSettings(It.IsAny<Guid>()))
            .ReturnsAsync(settings);
        authenticationRepository
            .Setup(r => r.FindUserSession(It.IsAny<Guid>()))
            .ReturnsAsync(session);
        dateTimeProvider
            .Setup(p => p.Now)
            .Returns(new DateTime(2018, 06, 11, 10, 10, 0));
        updateBuilder
            .Setup(b => b.Field(u => u.LastVisitDate, It.IsAny<DateTimeOffset>()))
            .Returns(updateBuilder.Object);

        var actual = await service.Authenticate("token");
        actual.Error.Should().Be(AuthenticationError.NoError);
        actual.User.Should().Be(user);
        actual.Session.Should().Be(session);
        actual.Settings.Should().Be(settings);
        actual.AuthenticationToken.Should().Be("token");

        cryptoService.Verify(s => s.Decrypt("token"));
        authenticationRepository.Verify(r => r.FindUser(userId), Times.Once);
        authenticationRepository.Verify(r => r.FindUserSettings(userId), Times.Once);
        authenticationRepository.Verify(r => r.FindUserSession(sessionId), Times.Once);
        authenticationRepository.Verify(r => r.UpdateActivity(updateBuilder.Object), Times.Once);
        authenticationRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SucceedWithoutExpirationCheckingWhenSessionIsPersistent()
    {
        var userId = Guid.Parse("7932d1d9-0a1a-4e16-b53f-5c213a2fc097");
        var sessionId = Guid.Parse("6f9e570c-1dca-4cca-be93-d7418b85959e");
        tokenDecryptSetup.ReturnsAsync(
            "{\"userId\": \"7932d1d9-0a1a-4e16-b53f-5c213a2fc097\"," +
            " \"sessionId\": \"6f9e570c-1dca-4cca-be93-d7418b85959e\"}");

        var user = new AuthenticatedUser();
        var settings = new UserSettings();
        var session = new Session
        {
            Id = sessionId,
            Persistent = true
        };
        authenticationRepository
            .Setup(r => r.FindUser(It.IsAny<Guid>()))
            .ReturnsAsync(user);
        authenticationRepository
            .Setup(r => r.FindUserSettings(It.IsAny<Guid>()))
            .ReturnsAsync(settings);
        authenticationRepository
            .Setup(r => r.FindUserSession(It.IsAny<Guid>()))
            .ReturnsAsync(session);
        updateBuilder
            .Setup(b => b.Field(u => u.LastVisitDate, It.IsAny<DateTimeOffset>()))
            .Returns(updateBuilder.Object);

        var actual = await service.Authenticate("token");
        actual.Error.Should().Be(AuthenticationError.NoError);
        actual.User.Should().Be(user);
        actual.Session.Should().Be(session);
        actual.Settings.Should().Be(settings);
        actual.AuthenticationToken.Should().Be("token");

        cryptoService.Verify(s => s.Decrypt("token"));
        authenticationRepository.Verify(r => r.FindUser(userId), Times.Once);
        authenticationRepository.Verify(r => r.FindUserSettings(userId), Times.Once);
        authenticationRepository.Verify(r => r.FindUserSession(sessionId), Times.Once);
        authenticationRepository.Verify(r => r.UpdateActivity(updateBuilder.Object), Times.Once);
        authenticationRepository.VerifyNoOtherCalls();
    }
}