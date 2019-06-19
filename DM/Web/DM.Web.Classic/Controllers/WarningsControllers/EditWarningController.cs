using System;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.Shared.Warnings.EditWarning;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.WarningsControllers
{
    public class EditWarningController : DmControllerBase
    {
        [HttpGet]
        public ActionResult Edit(Guid warningId)
        {
            if (Request.IsAjaxRequest())
            {
                /* TODO: ViewModelBuilder*/
            }
            return new EmptyResult();
        }

        [HttpPost, ValidationRequired]
        public ActionResult Edit(EditWarningForm editWarningForm)
        {
            if (Request.IsAjaxRequest())
            {
                /* TODO: ViewModelBuilder*/
            }
            return new EmptyResult();
        }
    }
}
