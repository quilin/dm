using System;
using System.Collections.Generic;
using DM.Services.DataAccess.BusinessObjects.Users;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Web.Core.Authentication
{
    public class FormAuthenticationService : IFormAuthenticationService
    {
        private readonly ISymmetricCryptoServiceProvider symmetricCryptoServiceProvider;
        private readonly ISessionProvider sessionProvider;
        private readonly IUserProvider userProvider;
        private readonly IUserService userService;

        private const string Key = "QkEeenXpHqgP6tOWwpUetAFvUUZiMb4f";
        private const string Iv = "dtEzMsz2ogg=";
        private const string HttpAuthorizationCookie = "__AUTH_cookie";
        private const string UserIdKey = "userId";
        private const string SessionIdKey = "sessionId";

        public FormAuthenticationService(
            ISymmetricCryptoServiceProvider symmetricCryptoServiceProvider,
            ISessionProvider sessionProvider,
            IUserProvider userProvider,
            IUserService userService)
        {
            this.symmetricCryptoServiceProvider = symmetricCryptoServiceProvider;
            this.sessionProvider = sessionProvider;
            this.userProvider = userProvider;
            this.userService = userService;
        }

        private void CreateAuthCookie(HttpContext httpContext, IUser user, Session session)
        {
            var createPersistentCookie = session.IsPersistent;

            var cookieData = JsonConvert.SerializeObject(new Dictionary<string, Guid>
            {
                {UserIdKey, user.UserId},
                {SessionIdKey, session.Id}
            });
            var encryptedCookieData = symmetricCryptoServiceProvider.GetTripleDes().Encrypt(cookieData, Key, Iv);

            httpContext.Response.Cookies.Append(HttpAuthorizationCookie, encryptedCookieData, new CookieOptions
            {
                Expires = createPersistentCookie ? session.ExpirationDate : (DateTimeOffset?) null,
                HttpOnly = true,
                IsEssential = true,
                Secure = false
            });
        }

        public void LogIn(HttpContext httpContext, Guid userId, bool createPersistentCookie, out Session session)
        {
            var user = userService.Read(userId);
            session = sessionProvider.CreateSessionForUser(user, createPersistentCookie);
            CreateAuthCookie(httpContext, user, session);
        }

        public void LogOut(HttpContext httpContext)
        {
            sessionProvider.RemoveSessionFromUser(userProvider.Current, userProvider.CurrentSession.Id);
            httpContext.Response.Cookies.Delete(HttpAuthorizationCookie);
        }

        public bool GetUserSession(HttpContext httpContext, out User user, out Session session)
        {
            if (!httpContext.Request.Cookies.TryGetValue(HttpAuthorizationCookie, out var authCookie))
            {
                user = User.Guest;
                session = null;
                return true;
            }

            Guid userId;
            Guid sessionId;
            var symmetricCryptoService = symmetricCryptoServiceProvider.GetTripleDes();
            try
            {
                var decryptedString = symmetricCryptoService.Decrypt(authCookie, Key, Iv);
                var cookieData = JsonConvert.DeserializeObject<Dictionary<string, Guid>>(decryptedString);
                userId = cookieData[UserIdKey];
                sessionId = cookieData[SessionIdKey];
            }
            catch (Exception)
            {
                user = User.Guest;
                session = null;
                return false;
            }

            user = userService.Read(userId);
            session = sessionProvider.GetSessionByIdForUser(user, sessionId);
            if (sessionProvider.RefreshUserSession(user, session))
            {
                CreateAuthCookie(httpContext, user, session);
            }
            return session != null;
        }
    }
}