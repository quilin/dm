using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Repositories;
using Newtonsoft.Json;

namespace DM.Services.Authentication.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ISecurityManager securityManager;
        private readonly ISymmetricCryptoService cryptoService;
        private readonly IAuthenticationRepository repository;

        private const string Key = "QkEeenXpHqgP6tOWwpUetAFvUUZiMb4f";
        private const string Iv = "dtEzMsz2ogg=";
        private const string UserIdKey = "userId";
        private const string SessionIdKey = "sessionId";

        public AuthenticationService(
            ISecurityManager securityManager,
            ISymmetricCryptoService cryptoService,
            IAuthenticationRepository repository)
        {
            this.securityManager = securityManager;
            this.cryptoService = cryptoService;
            this.repository = repository;
        }

        public Task<(AuthenticationError Error, AuthenticatingUser User)> Authenticate(
            string login, string password, string persistent)
        {
            throw new System.NotImplementedException();
        }

        public async Task<(AuthenticationError Error, AuthenticatingUser User)> Authenticate(string authToken)
        {
            try
            {
                var decryptedString = await cryptoService.Decrypt(authToken, Key, Iv);
                var data = JsonConvert.DeserializeObject<Dictionary<string, Guid>>(decryptedString);
                var userId = data[UserIdKey];
                var sessionId = data[SessionIdKey]; // todo: operate with session!!!
                var user = await repository.Find(userId);
                return (AuthenticationError.NoError, user);
            }
            catch
            {
                return (AuthenticationError.SessionExpired, null);
            }
        }
    }
}