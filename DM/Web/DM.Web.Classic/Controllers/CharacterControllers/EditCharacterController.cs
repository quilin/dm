using System;
using DM.Web.Classic.Dto;
using DM.Web.Classic.Views.CharactersList.Character;
using DM.Web.Classic.Views.EditCharacter;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CharacterControllers
{
    public class EditCharacterController : DmControllerBase
    {
        private readonly ICharacterEditFormBuilder characterEditFormBuilder;
        private readonly ICharacterService characterService;
        private readonly IModuleService moduleService;
        private readonly IIntentionsManager intentionsManager;
        private readonly ICharacterViewModelBuilder characterViewModelBuilder;
        private readonly IAttributesService attributesService;
        private readonly IUpdateCharacterModelConverter updateCharacterModelConverter;

        public EditCharacterController(
            ICharacterEditFormBuilder characterEditFormBuilder,
            ICharacterService characterService,
            IModuleService moduleService,
            IIntentionsManager intentionsManager,
            ICharacterViewModelBuilder characterViewModelBuilder,
            IAttributesService attributesService,
            IUpdateCharacterModelConverter updateCharacterModelConverter
            )
        {
            this.characterEditFormBuilder = characterEditFormBuilder;
            this.characterService = characterService;
            this.moduleService = moduleService;
            this.intentionsManager = intentionsManager;
            this.characterViewModelBuilder = characterViewModelBuilder;
            this.attributesService = attributesService;
            this.updateCharacterModelConverter = updateCharacterModelConverter;
        }

        [HttpGet]
        public ActionResult Edit(Guid characterId)
        {
            var character = characterService.Read(characterId);
            var module = moduleService.Read(character.ModuleId);
            intentionsManager.ThrowIfForbidden(CharacterIntention.Edit, character, module);

            var characterEditForm = characterEditFormBuilder.Build(character, module);
            return View("EditCharacter", characterEditForm);
        }

        [HttpPost, ValidationRequired]
        public ActionResult Edit(CharacterEditForm characterEditForm)
        {
            var updateCharacterModel = updateCharacterModelConverter.Convert(characterEditForm);
            characterService.Update(updateCharacterModel);

            var module = moduleService.Read(characterEditForm.ModuleId);
            return RedirectToAction("Index", "CharactersList", new { moduleId = module.ModuleId, characterId = characterEditForm.CharacterId });
        }

        [HttpPost]
        public ActionResult ChangeStatus(Guid characterId, CharacterIntention characterIntention)
        {
            var character = characterService.SetStatus(characterId, characterIntention);
            var module = moduleService.Read(character.ModuleId);

            var attributeSpecifications = new AttributeSpecification[0];
            var characterAttributes = new CharacterAttribute[0];
            if (module.AttributeSchemeId.HasValue)
            {
                attributeSpecifications = attributesService.ReadScheme(module.AttributeSchemeId.Value).Specifications;
                characterAttributes = attributesService.SelectCharacterAttributes(character.CharacterId);
            }
            var characterViewModel = characterViewModelBuilder.Build(character, module, attributeSpecifications, characterAttributes);
            return PartialView("~/Views/CharactersList/Character/CharacterPreview.cshtml", characterViewModel);
        }
    }
}