using System;
using DM.Web.Classic.Views.EditModuleStatus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.ModuleControllers
{
    public class EditModuleStatusController : DmControllerBase
    {
        private readonly IEditModuleStatusViewModelBuilder editModuleStatusViewModelBuilder;
        private readonly IModuleService moduleService;

        public EditModuleStatusController(
            IEditModuleStatusViewModelBuilder editModuleStatusViewModelBuilder,
            IModuleService moduleService
            )
        {
            this.editModuleStatusViewModelBuilder = editModuleStatusViewModelBuilder;
            this.moduleService = moduleService;
        }

        [HttpGet]
        public ActionResult EditStatus(Guid moduleId)
        {
            var editModuleStatusViewModel = editModuleStatusViewModelBuilder.Build(moduleId);
            return View("~/Views/EditModuleStatus/EditModuleStatus.cshtml", editModuleStatusViewModel);
        }

        [HttpPost, ValidationRequired]
        public string EditStatus(EditModuleStatusForm form)
        {
            var module = moduleService.ChangeStatus(form.ModuleId, form.Status);
            return Url.Action("Index", "Module", new RouteValueDictionary {{"moduleId", form.ModuleId.EncodeToReadable(module.Title)}});
        }

        [HttpPost]
        public string Premoderate(Guid moduleId)
        {
            var module = moduleService.TakeOnPremoderation(moduleId);
            return Url.Action("Index", "Module", new RouteValueDictionary {{"moduleId", moduleId.EncodeToReadable(module.Title)}});
        }
    }
}