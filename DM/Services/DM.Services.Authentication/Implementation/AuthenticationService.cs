using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Factories;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Authentication.Repositories;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using Newtonsoft.Json;

namespace DM.Services.Authentication.Implementation
{
    /// <inheritdoc />
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ISecurityManager securityManager;
        private readonly ISymmetricCryptoService cryptoService;
        private readonly IAuthenticationRepository repository;
        private readonly ISessionFactory sessionFactory;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IIdentityProvider identityProvider;

        private const string UserIdKey = "userId";
        private const string SessionIdKey = "sessionId";

        /// <inheritdoc />
        public AuthenticationService(
            ISecurityManager securityManager,
            ISymmetricCryptoService cryptoService,
            IAuthenticationRepository repository,
            ISessionFactory sessionFactory,
            IDateTimeProvider dateTimeProvider,
            IIdentityProvider identityProvider)
        {
            this.securityManager = securityManager;
            this.cryptoService = cryptoService;
            this.repository = repository;
            this.sessionFactory = sessionFactory;
            this.dateTimeProvider = dateTimeProvider;
            this.identityProvider = identityProvider;
        }

        /// <inheritdoc />
        public async Task<IIdentity> Authenticate(string login, string password, bool persistent)
        {
            var (userFound, user) = await repository.TryFindUser(login);
            switch (userFound)
            {
                case false:
                    return Identity.Fail(AuthenticationError.WrongLogin);
                case true when !user.Activated:
                    return Identity.Fail(AuthenticationError.Inactive);
                case true when user.IsRemoved:
                    return Identity.Fail(AuthenticationError.Removed);
                case true when user.AccessPolicy.HasFlag(AccessPolicy.FullBan):
                    return Identity.Fail(AuthenticationError.Banned);
                case true when !securityManager.ComparePasswords(password, user.Salt, user.PasswordHash):
                    // todo: brute force protection
                    return Identity.Fail(AuthenticationError.WrongPassword);

                default:
                    var session = sessionFactory.Create(persistent);
                    var settings = await repository.FindUserSettings(user.UserId);
                    return await CreateAuthenticationResult(user, session, settings);
            }
        }

        /// <inheritdoc />
        public async Task<IIdentity> Authenticate(string authToken)
        {
            Guid userId;
            Guid sessionId;

            try
            {
                var decryptedString = await cryptoService.Decrypt(authToken);
                var authData = JsonConvert.DeserializeObject<Dictionary<string, Guid>>(decryptedString);
                userId = authData[UserIdKey];
                sessionId = authData[SessionIdKey];
            }
            catch
            {
                return Identity.Fail(AuthenticationError.ForgedToken);
            }

            var fetchUser = repository.FindUser(userId);
            var fetchSession = repository.FindUserSession(sessionId);
            var fetchSettings = repository.FindUserSettings(userId);
            await Task.WhenAll(fetchUser, fetchSession, fetchSettings);

            var user = await fetchUser;
            var session = await fetchSession;
            var settings = await fetchSettings;
            if (!session.IsPersistent &&
                session.ExpirationDate < dateTimeProvider.Now)
            {
                await repository.RemoveSession(userId, sessionId);
                return Identity.Fail(AuthenticationError.SessionExpired);
            }

            var sessionRefreshDelta = TimeSpan.FromMinutes(20);
            if (!session.IsPersistent &&
                session.ExpirationDate - sessionRefreshDelta < dateTimeProvider.Now)
            {
                await repository.RefreshSession(userId, sessionId, session.ExpirationDate + sessionRefreshDelta);
            }

            return Identity.Success(user, session, settings, authToken);
        }

        /// <inheritdoc />
        public async Task<IIdentity> Authenticate(Guid userId)
        {
            var user = await repository.FindUser(userId);
            var session = sessionFactory.Create(false);
            var settings = await repository.FindUserSettings(userId);
            return await CreateAuthenticationResult(user, session, settings);
        }

        /// <inheritdoc />
        public async Task<IIdentity> Logout()
        {
            var identity = identityProvider.Current;
            await repository.RemoveSession(identity.User.UserId, identity.Session.Id);
            return Identity.Guest();
        }

        /// <inheritdoc />
        public async Task<IIdentity> LogoutAll()
        {
            var identity = identityProvider.Current;
            await repository.RemoveSessions(identity.User.UserId);

            var session = sessionFactory.Create(identity.Session.IsPersistent);
            return await CreateAuthenticationResult(identity.User, session, identity.Settings);
        }

        private async Task<IIdentity> CreateAuthenticationResult(
            AuthenticatedUser user, Session session, UserSettings settings)
        {
            await repository.AddSession(user.UserId, session);
            var authData = new Dictionary<string, Guid>
            {
                [UserIdKey] = user.UserId,
                [SessionIdKey] = session.Id
            };
            var token = await cryptoService.Encrypt(JsonConvert.SerializeObject(authData));
            return Identity.Success(user, session, settings, token);
        }
    }
}