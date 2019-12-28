using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Characters.Creating;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.CreateCharacter;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CharacterControllers
{
    public class CreateCharacterController : DmControllerBase
    {
        private readonly IGameReadingService gameService;
        private readonly ICharacterCreatingService characterService;
        private readonly IIntentionManager intentionManager;
        private readonly ICharacterCreateFormBuilder characterCreateFormBuilder;

        public CreateCharacterController(
            IGameReadingService gameService,
            ICharacterCreatingService characterService,
            IIntentionManager intentionManager,
            ICharacterCreateFormBuilder characterCreateFormBuilder)
        {
            this.gameService = gameService;
            this.characterService = characterService;
            this.intentionManager = intentionManager;
            this.characterCreateFormBuilder = characterCreateFormBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid gameId)
        {
            var game = await gameService.GetGameDetails(gameId);
            intentionManager.ThrowIfForbidden(GameIntention.CreateCharacter, game);

            var characterCreateForm = characterCreateFormBuilder.Build(game);
            return View("CreateCharacter", characterCreateForm);
        }

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> Create(CharacterCreateForm characterCreateForm)
        {
            var accessPolicy = CharacterAccessPolicy.NoAccess;
            if (characterCreateForm.MasterEditAllowed)
            {
                accessPolicy |= CharacterAccessPolicy.EditAllowed;
            }

            if (characterCreateForm.MasterPostsEditAllowed)
            {
                accessPolicy |= CharacterAccessPolicy.PostEditAllowed;
            }
            
            var createdCharacter = await characterService.Create(new CreateCharacter
            {
                GameId = characterCreateForm.GameId,
                Name = characterCreateForm.Name,
                Race = characterCreateForm.Race,
                Class = characterCreateForm.Class,
                Alignment = characterCreateForm.Alignment,
                Appearance = characterCreateForm.Appearance,
                Temper = characterCreateForm.Temper,
                Story = characterCreateForm.Story,
                Skills = characterCreateForm.Name,
                Inventory = characterCreateForm.Inventory,
                IsNpc = characterCreateForm.IsNpc,
                AccessPolicy = accessPolicy,
            });
            return RedirectToAction("Index", "CharactersList", new
            {
                gameId = characterCreateForm.GameId,
                characterId = createdCharacter.Id
            });
        }
    }
}