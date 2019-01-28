using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Factories;
using DM.Services.Authentication.Implementation;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Repositories;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Testing.Core;
using FluentAssertions;
using Moq;
using Xunit;

namespace DM.Services.Authentication.Tests
{
    public class AuthenticationServiceShould : UnitTestBase
    {
        private readonly Mock<ISecurityManager> securityManager;
        private readonly Mock<ISymmetricCryptoService> cryptoService;
        private readonly Mock<IAuthenticationRepository> repository;
        private readonly Mock<ISessionFactory> sessionFactory;
        private readonly Mock<IDateTimeProvider> dateTimeProvider;
        private readonly AuthenticationService authenticationService;

        public AuthenticationServiceShould()
        {
            securityManager = Mock<ISecurityManager>();
            cryptoService = Mock<ISymmetricCryptoService>();
            repository = Mock<IAuthenticationRepository>();
            sessionFactory = Mock<ISessionFactory>();
            dateTimeProvider = Mock<IDateTimeProvider>();
            authenticationService = new AuthenticationService(securityManager.Object, cryptoService.Object,
                repository.Object, sessionFactory.Object, dateTimeProvider.Object);
        }

        [Fact]
        public async Task ReturnFailureWhenNoUserWithLogin()
        {
            repository
                .Setup(r => r.TryFindUser(It.IsAny<string>()))
                .ReturnsAsync((false, null));

            var actual = await authenticationService.Authenticate("login", "password", false);

            actual.Error.Should().Be(AuthenticationError.WrongLogin);
            repository.Verify(r => r.TryFindUser("login"), Times.Once);
        }

        [Fact]
        public async Task ReturnFailureWhenUserNotActivated()
        {
            var user = new AuthenticatedUser {Activated = false};
            repository
                .Setup(r => r.TryFindUser(It.IsAny<string>()))
                .ReturnsAsync((true, user));

            var actual = await authenticationService.Authenticate("login", "password", false);

            actual.Error.Should().Be(AuthenticationError.Inactive);
            repository.Verify(r => r.TryFindUser("login"), Times.Once);
        }

        [Fact]
        public async Task ReturnFailureWhenUserRemoved()
        {
            var user = new AuthenticatedUser {Activated = true, IsRemoved = true};
            repository
                .Setup(r => r.TryFindUser(It.IsAny<string>()))
                .ReturnsAsync((true, user));

            var actual = await authenticationService.Authenticate("login", "password", false);

            actual.Error.Should().Be(AuthenticationError.Removed);
            repository.Verify(r => r.TryFindUser("login"), Times.Once);
        }

        [Fact]
        public async Task ReturnFailureWhenUserBanned()
        {
            var user = new AuthenticatedUser
            {
                Activated = true,
                IsRemoved = false,
                AccessPolicy = AccessPolicy.DemocraticBan | AccessPolicy.FullBan
            };
            repository
                .Setup(r => r.TryFindUser(It.IsAny<string>()))
                .ReturnsAsync((true, user));

            var actual = await authenticationService.Authenticate("login", "password", false);

            actual.Error.Should().Be(AuthenticationError.Banned);
            repository.Verify(r => r.TryFindUser("login"), Times.Once);
        }

        [Fact]
        public async Task ReturnFailureWhenPasswordDontMatch()
        {
            var user = new AuthenticatedUser
            {
                Activated = true,
                IsRemoved = false,
                AccessPolicy = AccessPolicy.DemocraticBan,
                Salt = "salt",
                PasswordHash = "hash"
            };
            repository
                .Setup(r => r.TryFindUser(It.IsAny<string>()))
                .ReturnsAsync((true, user));

            securityManager
                .Setup(m => m.ComparePasswords(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            var actual = await authenticationService.Authenticate("login", "password", false);

            actual.Error.Should().Be(AuthenticationError.WrongPassword);
            repository.Verify(r => r.TryFindUser("login"), Times.Once);
            securityManager.Verify(m => m.ComparePasswords("password", "salt", "hash"), Times.Once);
        }

        [Fact]
        public async Task ReturnSuccessWhenPasswordMatch()
        {
            var userId = Guid.NewGuid();
            var user = new AuthenticatedUser
            {
                UserId = userId,
                Activated = true,
                IsRemoved = false,
                AccessPolicy = AccessPolicy.DemocraticBan,
                Salt = "salt",
                PasswordHash = "hash"
            };
            repository
                .Setup(r => r.TryFindUser(It.IsAny<string>()))
                .ReturnsAsync((true, user));
            securityManager
                .Setup(m => m.ComparePasswords(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            var session = new Session();
            sessionFactory
                .Setup(f => f.Create(It.IsAny<bool>()))
                .Returns(session);
            repository
                .Setup(r => r.AddSession(It.IsAny<Guid>(), It.IsAny<Session>()))
                .Returns(Task.CompletedTask);
            cryptoService
                .Setup(s => s.Encrypt(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("token");

            var actual = await authenticationService.Authenticate("login", "password", false);

            actual.Error.Should().Be(AuthenticationError.NoError);
            actual.User.Should().Be(user);
            actual.Session.Should().Be(session);
            actual.Token.Should().Be("token");

            repository.Verify(r => r.TryFindUser("login"), Times.Once);
            repository.Verify(r => r.AddSession(userId, session), Times.Once);
            securityManager.Verify(m => m.ComparePasswords("password", "salt", "hash"), Times.Once);
        }

        [Fact]
        public async Task ReturnFailureWhenTokenForged()
        {
            cryptoService
                .Setup(s => s.Decrypt(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("something wrong");

            var actual = await authenticationService.Authenticate("token");

            actual.Error.Should().Be(AuthenticationError.SessionExpired);
        }

        [Fact]
        public async Task ReturnFailureWhenSessionExpired()
        {
            var userId = Guid.Parse("6dc78af8-edea-4718-a408-a16d065cd006");
            var sessionId = Guid.Parse("35b3982c-7faf-4a5b-8690-6d8fc6498188");
            cryptoService
                .Setup(s => s.Decrypt(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(
                    "{\"userId\":\"6dc78af8-edea-4718-a408-a16d065cd006\",\"sessionId\":\"35b3982c-7faf-4a5b-8690-6d8fc6498188\"}");
            var session = new Session
            {
                Id = sessionId,
                IsPersistent = false,
                ExpirationDate = new DateTime(2010, 01, 01)
            };
            repository
                .Setup(r => r.FindUser(It.IsAny<Guid>()))
                .ReturnsAsync(new AuthenticatedUser());
            repository
                .Setup(r => r.FindUserSession(It.IsAny<Guid>()))
                .ReturnsAsync(session);
            repository
                .Setup(r => r.RemoveSession(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);
            dateTimeProvider
                .Setup(p => p.Now)
                .Returns(new DateTime(2018, 08, 21));

            var actual = await authenticationService.Authenticate("token");

            actual.Error.Should().Be(AuthenticationError.SessionExpired);
            repository.Verify(r => r.FindUser(userId), Times.Once);
            repository.Verify(r => r.FindUserSession(sessionId), Times.Once);
            repository.Verify(r => r.RemoveSession(userId, sessionId), Times.Once);
        }

        [Fact]
        public async Task ReturnSuccessAndRefreshSessionWhenTokenAboutToExpire()
        {
            var userId = Guid.Parse("c1e28c24-010f-4267-970f-487a7f5e2593");
            var sessionId = Guid.Parse("ddd49a53-7623-445c-b2ee-e429a4c6f08b");
            cryptoService
                .Setup(s => s.Decrypt(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(
                    "{\"userId\":\"c1e28c24-010f-4267-970f-487a7f5e2593\",\"sessionId\":\"ddd49a53-7623-445c-b2ee-e429a4c6f08b\"}");
            var session = new Session
            {
                Id = sessionId,
                IsPersistent = false,
                ExpirationDate = new DateTime(2018, 03, 02, 18, 30, 55)
            };
            var user = new AuthenticatedUser();
            repository
                .Setup(r => r.FindUser(It.IsAny<Guid>()))
                .ReturnsAsync(user);
            repository
                .Setup(r => r.FindUserSession(It.IsAny<Guid>()))
                .ReturnsAsync(session);
            repository
                .Setup(r => r.RefreshSession(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateTime>()))
                .Returns(Task.CompletedTask);
            dateTimeProvider
                .Setup(p => p.Now)
                .Returns(new DateTime(2018, 03, 02, 18, 15, 10));

            var actual = await authenticationService.Authenticate("token");

            actual.Error.Should().Be(AuthenticationError.NoError);
            actual.User.Should().Be(user);
            actual.Session.Should().Be(session);
            actual.Token.Should().Be("token");

            repository.Verify(r => r.FindUser(userId), Times.Once);
            repository.Verify(r => r.FindUserSession(sessionId), Times.Once);
            repository.Verify(r => r.RefreshSession(userId, sessionId, new DateTime(2018, 03, 02, 18, 50, 55)), Times.Once);
        }

        public override void Dispose()
        {
            repository.VerifyNoOtherCalls();
            securityManager.VerifyNoOtherCalls();
            base.Dispose();
        }
    }
}