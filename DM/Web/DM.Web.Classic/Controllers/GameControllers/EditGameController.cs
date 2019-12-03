using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameControllers
{
    public class EditGameController : DmControllerBase
    {
        private readonly IModuleService moduleService;
        private readonly IUpdateModuleModelConverter updateModuleModelConverter;

        public EditGameController(
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
            return RedirectToAction("Index", "Game", new { moduleId = editModuleForm.ModuleId });
        }
    }
}