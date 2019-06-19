using DM.Web.Classic.Dto;
using DM.Web.Classic.Views.EditModule;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ModuleControllers
{
    public class EditModuleController : DmControllerBase
    {
        private readonly IModuleService moduleService;
        private readonly IUpdateModuleModelConverter updateModuleModelConverter;

        public EditModuleController(
            IModuleService moduleService,
            IUpdateModuleModelConverter updateModuleModelConverter)
        {
            this.moduleService = moduleService;
            this.updateModuleModelConverter = updateModuleModelConverter;
        }

        [HttpPost, ValidationRequired]
        public ActionResult SaveChanges(EditModuleForm editModuleForm)
        {
            var updateModuleModel = updateModuleModelConverter.Convert(editModuleForm);
            moduleService.Update(updateModuleModel);
            return RedirectToAction("Index", "Module", new { moduleId = editModuleForm.ModuleId });
        }
    }
}