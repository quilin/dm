using DM.Web.Classic.Dto;
using DM.Web.Classic.Views.CreateModule;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ModuleControllers
{
    public class CreateModuleController : DmControllerBase
    {
        private readonly ICreateModuleViewModelBuilder createModuleViewModelBuilder;
        private readonly ICreateModuleModelConverter createModuleModelConverter;
        private readonly IIntentionsManager intentionsManager;
        private readonly IModuleService moduleService;

        public CreateModuleController(
            ICreateModuleViewModelBuilder createModuleViewModelBuilder,
            ICreateModuleModelConverter createModuleModelConverter,
            IIntentionsManager intentionsManager,
            IModuleService moduleService
            )
        {
            this.createModuleViewModelBuilder = createModuleViewModelBuilder;
            this.createModuleModelConverter = createModuleModelConverter;
            this.intentionsManager = intentionsManager;
            this.moduleService = moduleService;
        }

        [HttpGet]
        public ActionResult Create()
        {
            intentionsManager.ThrowIfForbidden(CommonIntention.CreateModule);

            var createModuleViewModel = createModuleViewModelBuilder.Build();
            return View("CreateModule", createModuleViewModel);
        }

        [HttpPost, ValidationRequired]
        public ActionResult Create(CreateModuleForm createModuleForm)
        {
            var createModuleModel = createModuleModelConverter.Convert(createModuleForm);
            var module = moduleService.Create(createModuleModel);
            return RedirectToAction("Index", "Module", new { moduleId = module.ModuleId.EncodeToReadable(module.Title) });
        }
    }
}