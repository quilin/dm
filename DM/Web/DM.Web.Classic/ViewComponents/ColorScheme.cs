using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.ViewComponents
{
    public class ColorScheme : ViewComponent
    {
        private readonly IIdentity identity;

        public ColorScheme(
            IIdentityProvider identityProvider)
        {
            identity = identityProvider.Current;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.Run(() =>
                Content($"colorscheme_{identity.Settings.ColorScheme.ToString().ToLower()}"));
        }
    }
}