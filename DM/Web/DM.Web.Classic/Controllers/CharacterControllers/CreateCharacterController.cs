using System;
using DM.Web.Classic.Dto;
using DM.Web.Classic.Views.CreateCharacter;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CharacterControllers
{
    public class CreateCharacterController : DmControllerBase
    {
        private readonly ICharacterCreateFormBuilder characterCreateFormBuilder;
        private readonly IIntentionsManager intentionsManager;
        private readonly ICharacterService characterService;
        private readonly IModuleService moduleService;
        private readonly ICreateCharacterModelConverter createCharacterModelConverter;

        public CreateCharacterController(
            ICharacterCreateFormBuilder characterCreateFormBuilder,
            IIntentionsManager intentionsManager,
            ICharacterService characterService,
            IModuleService moduleService,
            ICreateCharacterModelConverter createCharacterModelConverter
            )
        {
            this.characterCreateFormBuilder = characterCreateFormBuilder;
            this.intentionsManager = intentionsManager;
            this.characterService = characterService;
            this.moduleService = moduleService;
            this.createCharacterModelConverter = createCharacterModelConverter;
        }

        [HttpGet]
        public ActionResult Create(Guid moduleId)
        {
            var module = moduleService.Read(moduleId);
            intentionsManager.ThrowIfForbidden(ModuleIntention.CreateCharacter, module);

            var characterCreateForm = characterCreateFormBuilder.Build(module);
            return View("CreateCharacter", characterCreateForm);
        }

        [HttpPost, ValidationRequired]
        public ActionResult Create(CharacterCreateForm characterCreateForm)
        {
            var createCharacterModel = createCharacterModelConverter.Convert(characterCreateForm);
            var character = characterService.Create(createCharacterModel);
            return RedirectToAction("Index", "CharactersList", new { moduleId = characterCreateForm.ModuleId, characterId = character.CharacterId });
        }
    }
}