using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Repositories;
using DM.Services.Core.Implementation;
using Newtonsoft.Json;

namespace DM.Services.Authentication.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ISecurityManager securityManager;
        private readonly ISymmetricCryptoService cryptoService;
        private readonly IAuthenticationRepository repository;
        private readonly IDateTimeProvider dateTimeProvider;

        private const string Key = "QkEeenXpHqgP6tOWwpUetAFvUUZiMb4f";
        private const string Iv = "dtEzMsz2ogg=";
        private const string UserIdKey = "userId";
        private const string SessionIdKey = "sessionId";

        public AuthenticationService(
            ISecurityManager securityManager,
            ISymmetricCryptoService cryptoService,
            IAuthenticationRepository repository,
            IDateTimeProvider dateTimeProvider)
        {
            this.securityManager = securityManager;
            this.cryptoService = cryptoService;
            this.repository = repository;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<(AuthenticationError Error, AuthenticatingUser User)> Authenticate(
            string login, string password, string persistent)
        {
            var (userFound, user) = await repository.TryFindUser(login);
            switch (userFound)
            {
                case false:
                    return (AuthenticationError.WrongLogin, user);
                case true when !user.Activated:
                    return (AuthenticationError.Inactive, user);
                case true when user.IsRemoved:
                    return (AuthenticationError.Removed, user);
                case true when !securityManager.ComparePasswords(password, user.Salt, user.PasswordHash):
                    return (AuthenticationError.WrongPassword, user);
                // todo: banned user?
                default:
                    return (AuthenticationError.NoError, user);
            }
        }

        public async Task<(AuthenticationError Error, AuthenticatingUser User)> Authenticate(string authToken)
        {
            try
            {
                var decryptedString = await cryptoService.Decrypt(authToken, Key, Iv);
                var data = JsonConvert.DeserializeObject<Dictionary<string, Guid>>(decryptedString);
                var userId = data[UserIdKey];
                var sessionId = data[SessionIdKey];

                var fetchUser = repository.FindUser(userId);
                var fetchSession = repository.FindUserSession(sessionId);
                await Task.WhenAll(fetchUser, fetchSession);

                var user = await fetchUser;
                var session = await fetchSession;
                if (!session.IsPersistent && session.ExpirationDate < dateTimeProvider.Now)
                {
                    await repository.RemoveSession(session);
                    return (AuthenticationError.SessionExpired, user);
                }

                await repository.RefreshSession(session);
            }
            catch
            {
                return (AuthenticationError.SessionExpired, null);
            }
        }
    }
}