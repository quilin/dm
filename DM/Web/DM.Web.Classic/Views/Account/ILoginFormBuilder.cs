using Microsoft.AspNetCore.Http;

namespace DM.Web.Classic.Views.Account
{
    public interface ILoginFormBuilder
    {
        LoginForm Build(HttpRequest request);
    }
}