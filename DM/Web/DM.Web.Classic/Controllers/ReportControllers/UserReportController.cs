using DM.Web.Classic.Views.Profile.ReportUser;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ReportControllers
{
    public class UserReportController : DmControllerBase
    {
        private readonly IReportService reportService;
        private readonly IIntentionsManager intentionsManager;

        public UserReportController(
            IReportService reportService,
            IIntentionsManager intentionsManager
            )
        {
            this.reportService = reportService;
            this.intentionsManager = intentionsManager;
        }

        [HttpPost, ValidationRequired]
        public ActionResult Report(ReportUserForm form)
        {
            reportService.ReportUser(form.UserId, form.Text);
            var canReport = intentionsManager.IsAllowed(CommonIntention.Report);
            return Json(new {canReport});
        }
    }
}