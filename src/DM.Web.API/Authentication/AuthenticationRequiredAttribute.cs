using System.Net;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DM.Web.API.Authentication;

/// <inheritdoc />
internal class AuthenticationRequiredAttribute : TypeFilterAttribute
{
    /// <inheritdoc />
    public AuthenticationRequiredAttribute() : base(typeof(AuthenticationRequiredFilter))
    {
    }

    private class AuthenticationRequiredFilter(IIdentityProvider identityProvider) : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!identityProvider.Current.User.IsAuthenticated)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "User must be authenticated");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}