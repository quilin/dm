using DM.Web.Classic.Views.MergeProfile;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ProfileControllers
{
    public class MergeProfileController : DmControllerBase
    {
        private readonly IUserService userService;
        private readonly IInitiateMergeFormBuilder initiateMergeFormBuilder;
        private readonly ICompleteMergeFormBuilder completeMergeFormBuilder;

        public MergeProfileController(
            IUserService userService,
            IInitiateMergeFormBuilder initiateMergeFormBuilder,
            ICompleteMergeFormBuilder completeMergeFormBuilder
        )
        {
            this.userService = userService;
            this.initiateMergeFormBuilder = initiateMergeFormBuilder;
            this.completeMergeFormBuilder = completeMergeFormBuilder;
        }

        [HttpGet]
        public ActionResult InitiateMerge()
        {
            var initiateMergeForm = initiateMergeFormBuilder.Build();
            return View("InitiateMerge", initiateMergeForm);
        }

        [HttpPost, ValidationRequired]
        public IActionResult InitiateMerge(InitiateMergeForm form)
        {
            return userService.TryInitiateProfileMerge(form.NewAccountPassword,
                form.OldAccountLogin ?? string.Empty, form.OldAccountPassword, out var error)
                ? new EmptyResult()
                : AjaxFormError(error);
        }

        [HttpGet]
        public ActionResult CompleteMerge()
        {
            var completeMergeForm = completeMergeFormBuilder.Build();
            return View("CompleteMerge", completeMergeForm);
        }

        [HttpPost, ValidationRequired]
        public IActionResult CompleteMerge(CompleteMergeForm form)
        {
            return userService.TryCompleteProfileMerge(form.Password, out var error)
                ? new EmptyResult()
                : AjaxFormError(error);
        }
    }
}