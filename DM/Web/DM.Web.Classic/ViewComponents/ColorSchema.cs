using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.ViewComponents
{
    public class ColorSchema : ViewComponent
    {
        private readonly IIdentityProvider identityProvider;

        public ColorSchema(
            IIdentityProvider identityProvider)
        {
            this.identityProvider = identityProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return Content($"colorschema_{identityProvider.Current.Settings.ColorSchema.ToString().ToLower()}");
        }
    }
}