using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.ViewComponents
{
    public class ColorSchema : ViewComponent
    {
        private readonly IIdentity identity;

        public ColorSchema(
            IIdentityProvider identityProvider)
        {
            identity = identityProvider.Current;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.Run(() =>
                Content($"colorschema_{identity.Settings.ColorSchema.ToString().ToLower()}"));
        }
    }
}