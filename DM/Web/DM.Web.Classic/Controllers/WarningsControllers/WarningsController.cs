using System;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.Shared.Warnings;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.WarningsControllers
{
    public class WarningsController : DmControllerBase
    {
        private readonly IWarningsListViewModelBuilder warningsListViewModelBuilder;
        private readonly IWarningService warningService;

        public WarningsController(
            IWarningsListViewModelBuilder warningsListViewModelBuilder,
            IWarningService warningService
        )
        {
            this.warningsListViewModelBuilder = warningsListViewModelBuilder;
            this.warningService = warningService;
        }

        public ActionResult ListEntityWarnings(Guid entityId)
        {
            var warningsListViewModel = warningsListViewModelBuilder.Build(entityId);
            return PartialView("~/Views/Shared/Warnings/WarningsList.cshtml", warningsListViewModel);
        }

        public ActionResult ListUserWarnings(Guid userId)
        {
            if (Request.IsAjaxRequest())
            {
                /* TODO: ViewModelBuilder*/
            }

            return new EmptyResult();
        }

        public ActionResult ListRecentWarnings(int? page)
        {
            if (Request.IsAjaxRequest())
            {
                /* TODO: ViewModelBuilder*/
            }

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult Remove(Guid warningId)
        {
            warningService.Remove(warningId);
            return null;
        }
    }
}