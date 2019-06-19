using Microsoft.AspNetCore.Http;

namespace DM.Web.Classic.Views.Account
{
    public interface IRegistrationFormBuilder
    {
        RegistrationForm Build(HttpRequest request);
    }
}