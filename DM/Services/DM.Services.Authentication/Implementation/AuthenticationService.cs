using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Factories;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Repositories;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using Newtonsoft.Json;

namespace DM.Services.Authentication.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ISecurityManager securityManager;
        private readonly ISymmetricCryptoService cryptoService;
        private readonly IAuthenticationRepository repository;
        private readonly ISessionFactory sessionFactory;
        private readonly IDateTimeProvider dateTimeProvider;

        private const string Key = "QkEeenXpHqgP6tOWwpUetAFvUUZiMb4f";
        private const string Iv = "dtEzMsz2ogg=";
        private const string UserIdKey = "userId";
        private const string SessionIdKey = "sessionId";

        public AuthenticationService(
            ISecurityManager securityManager,
            ISymmetricCryptoService cryptoService,
            IAuthenticationRepository repository,
            ISessionFactory sessionFactory,
            IDateTimeProvider dateTimeProvider)
        {
            this.securityManager = securityManager;
            this.cryptoService = cryptoService;
            this.repository = repository;
            this.sessionFactory = sessionFactory;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<AuthenticationResult> Authenticate(string login, string password, bool persistent)
        {
            var (userFound, user) = await repository.TryFindUser(login);
            switch (userFound)
            {
                case false:
                    return AuthenticationResult.Fail(AuthenticationError.WrongLogin);
                case true when !user.Activated:
                    return AuthenticationResult.Fail(AuthenticationError.Inactive);
                case true when user.IsRemoved:
                    return AuthenticationResult.Fail(AuthenticationError.Removed);
                case true when user.AccessPolicy.HasFlag(AccessPolicy.FullBan):
                    return AuthenticationResult.Fail(AuthenticationError.Banned);
                case true when !securityManager.ComparePasswords(password, user.Salt, user.PasswordHash):
                    return AuthenticationResult.Fail(AuthenticationError.WrongPassword);

                default:
                    var session = sessionFactory.Create(persistent);
                    await repository.AddSession(user.UserId, session);
                    var authData = new Dictionary<string, Guid>
                    {
                        [UserIdKey] = user.UserId,
                        [SessionIdKey] = session.Id
                    };
                    var token = await cryptoService.Encrypt(JsonConvert.SerializeObject(authData), Key, Iv);
                    return AuthenticationResult.Success(user, session, token);
            }
        }

        public async Task<AuthenticationResult> Authenticate(string authToken)
        {
            Guid userId;
            Guid sessionId;
            
            try
            {
                var decryptedString = await cryptoService.Decrypt(authToken, Key, Iv);
                var authData = JsonConvert.DeserializeObject<Dictionary<string, Guid>>(decryptedString);
                userId = authData[UserIdKey];
                sessionId = authData[SessionIdKey];
            }
            catch
            {
                return AuthenticationResult.Fail(AuthenticationError.SessionExpired);
            }

            var fetchUser = repository.FindUser(userId);
            var fetchSession = repository.FindUserSession(sessionId);
            await Task.WhenAll(fetchUser, fetchSession);

            var user = await fetchUser;
            var session = await fetchSession;
            if (!session.IsPersistent &&
                session.ExpirationDate < dateTimeProvider.Now)
            {
                await repository.RemoveSession(userId, sessionId);
                return AuthenticationResult.Fail(AuthenticationError.SessionExpired);
            }

            var sessionRefreshDelta = TimeSpan.FromMinutes(20);
            if (!session.IsPersistent &&
                session.ExpirationDate - sessionRefreshDelta < dateTimeProvider.Now)
            {
                await repository.RefreshSession(userId, sessionId, session.ExpirationDate + sessionRefreshDelta);
            }

            return AuthenticationResult.Success(user, session, authToken);
        }
    }
}