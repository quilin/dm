using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.CreateGame;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameControllers
{
    public class CreateGameController : DmControllerBase
    {
        private readonly ICreateGameViewModelBuilder createGameViewModelBuilder;
        private readonly ICreateModuleModelConverter createModuleModelConverter;
        private readonly IIntentionsManager intentionsManager;
        private readonly IModuleService moduleService;

        public CreateGameController(
            ICreateGameViewModelBuilder createGameViewModelBuilder,
            ICreateModuleModelConverter createModuleModelConverter,
            IIntentionsManager intentionsManager,
            IModuleService moduleService
            )
        {
            this.createGameViewModelBuilder = createGameViewModelBuilder;
            this.createModuleModelConverter = createModuleModelConverter;
            this.intentionsManager = intentionsManager;
            this.moduleService = moduleService;
        }

        [HttpGet]
        public ActionResult Create()
        {
            intentionsManager.ThrowIfForbidden(CommonIntention.CreateModule);

            var createModuleViewModel = createGameViewModelBuilder.Build();
            return View("CreateModule", createModuleViewModel);
        }

        [HttpPost, ValidationRequired]
        public ActionResult Create(CreateGameForm createGameForm)
        {
            var createModuleModel = createModuleModelConverter.Convert(createGameForm);
            var module = moduleService.Create(createModuleModel);
            return RedirectToAction("Index", "Game", new { moduleId = module.ModuleId.EncodeToReadable(module.Title) });
        }
    }
}