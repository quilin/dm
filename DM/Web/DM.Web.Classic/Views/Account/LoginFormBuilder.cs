using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace DM.Web.Classic.Views.Account
{
    public class LoginFormBuilder : ILoginFormBuilder
    {
        public LoginForm Build(HttpRequest request)
        {
            return new LoginForm
            {
                RedirectUrl = request.GetEncodedUrl()
            };
        }
    }
}