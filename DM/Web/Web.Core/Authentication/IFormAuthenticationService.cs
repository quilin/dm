using System;
using DM.Services.DataAccess.BusinessObjects.Users;
using Microsoft.AspNetCore.Http;

namespace Web.Core.Authentication
{
    public interface IFormAuthenticationService
    {
        void LogIn(HttpContext httpContext, Guid userId, bool createPersistentCookie, out Session session);
        void LogOut(HttpContext httpContext);
        bool GetUserSession(HttpContext httpContext, out User user, out Session session);
    }
}