using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Factories;
using DM.Services.Authentication.Implementation;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Repositories;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Tests.Core;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Authentication.Tests
{
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
                authenticationRepository.Object, sessionFactory.Object, null, identityProvider.Object);
        }

        [Fact]
        public async Task RemoveActiveUserSession()
        {
            var userId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            userSetup.Returns(new AuthenticatedUser {UserId = userId});
            sessionSetup.Returns(new Session {Id = sessionId});
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
            var session = new Session {IsPersistent = true};
            var userSettings = new UserSettings();
            userSetup.Returns(user);
            sessionSetup.Returns(session);
            identity.Setup(i => i.Settings).Returns(userSettings);
            authenticationRepository
                .Setup(r => r.RemoveSessions(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);
            authenticationRepository
                .Setup(r => r.AddSession(It.IsAny<Guid>(), It.IsAny<Session>()))
                .Returns(Task.CompletedTask);
            cryptoService
                .Setup(s => s.Encrypt(It.IsAny<string>()))
                .ReturnsAsync("token");

            var newSession = new Session();
            sessionFactory.Setup(f => f.Create(true)).Returns(newSession);

            var actual = await service.LogoutAll();
            actual.Error.Should().Be(AuthenticationError.NoError);
            actual.User.Should().Be(user);
            actual.Session.Should().Be(newSession);
            actual.Settings.Should().Be(userSettings);
            actual.AuthenticationToken.Should().Be("token");

            authenticationRepository.Verify(r => r.RemoveSessions(userId), Times.Once);
            authenticationRepository.Verify(r => r.AddSession(userId, newSession), Times.Once);
            authenticationRepository.VerifyNoOtherCalls();
        }
    }
}