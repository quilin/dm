using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace DM.Web.Classic.Views.Account
{
    public class RegistrationFormBuilder : IRegistrationFormBuilder
    {
        public RegistrationForm Build(HttpRequest request)
        {
            return new RegistrationForm
            {
                RedirectUrl = request.GetEncodedUrl()
            };
        }
    }
}