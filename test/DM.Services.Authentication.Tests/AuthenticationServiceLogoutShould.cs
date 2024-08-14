using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Factories;
using DM.Services.Authentication.Implementation;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Authentication.Repositories;
using DM.Tests.Core;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Xunit;
using DbSession = DM.Services.DataAccess.BusinessObjects.Users.Session;

namespace DM.Services.Authentication.Tests;

public class AuthenticationServiceLogoutShould : UnitTestBase
{
    private readonly ISetup<IIdentity, AuthenticatedUser> userSetup;
    private readonly ISetup<IIdentity, Session> sessionSetup;
    private readonly AuthenticationService service;
    private readonly Mock<IAuthenticationRepository> authenticationRepository;
    private readonly Mock<ISessionFactory> sessionFactory;
    private readonly Mock<IIdentity> identity;
    private readonly Mock<ISymmetricCryptoService> cryptoService;

    public AuthenticationServiceLogoutShould()
    {
        authenticationRepository = Mock<IAuthenticationRepository>();
        sessionFactory = Mock<ISessionFactory>();
        cryptoService = Mock<ISymmetricCryptoService>();
        var identityProvider = Mock<IIdentityProvider>();
        identity = Mock<IIdentity>();
        identityProvider
            .Setup(p => p.Current)
            .Returns(identity.Object);
        userSetup = identity.Setup(i => i.User);
        sessionSetup = identity.Setup(i => i.Session);
        service = new AuthenticationService(null, cryptoService.Object,
            authenticationRepository.Object, sessionFactory.Object, null, identityProvider.Object, null);
    }

    [Fact]
    public async Task RemoveActiveUserSession()
    {
        var userId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var session = new Session{Id = sessionId};
        userSetup.Returns(new AuthenticatedUser {UserId = userId});
        sessionSetup.Returns(session);
        authenticationRepository
            .Setup(r => r.RemoveSession(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);
        await service.Logout();

        authenticationRepository.Verify(r => r.RemoveSession(userId, sessionId), Times.Once);
        authenticationRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RemoveAllActiveSessionsAndCreateNewSimilarToCurrent()
    {
        var userId = Guid.NewGuid();
        var user = new AuthenticatedUser {UserId = userId};
        var sessionId = Guid.NewGuid();
        var session = new Session{Id = sessionId};
        var userSettings = new UserSettings();
        userSetup.Returns(user);
        sessionSetup.Returns(session);
        identity.Setup(i => i.Settings).Returns(userSettings);
        authenticationRepository
            .Setup(r => r.RemoveSessionsExcept(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);
        cryptoService
            .Setup(s => s.Encrypt(It.IsAny<string>()))
            .ReturnsAsync("token");

        var newSessionId = Guid.NewGuid();
        var newSession = new Session {Id = newSessionId};
        var sessionToCreate = new DbSession{Id = sessionId};
        sessionFactory.Setup(f => f.Create(false, false)).Returns(sessionToCreate);
        authenticationRepository
            .Setup(r => r.AddSession(It.IsAny<Guid>(), It.IsAny<DbSession>()))
            .ReturnsAsync(newSession);

        await service.Invoking(s => s.LogoutElsewhere()).Should().NotThrowAsync();

        authenticationRepository.Verify(r => r.RemoveSessionsExcept(userId, sessionId), Times.Once);
        authenticationRepository.VerifyNoOtherCalls();
    }
}