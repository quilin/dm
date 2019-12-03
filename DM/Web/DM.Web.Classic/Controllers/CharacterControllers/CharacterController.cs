using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.BusinessProcesses.Characters.Deleting;
using DM.Services.Gaming.BusinessProcesses.Claims.Creating;
using DM.Services.Gaming.BusinessProcesses.Claims.Deleting;
using DM.Services.Gaming.Dto.Input;
using Microsoft.AspNetCore.Mvc;
using CharacterViewModelBuilder = DM.Web.Classic.Views.CharactersList.Character.ICharacterViewModelBuilder;
using ListCharacterViewModelBuilder = DM.Web.Classic.Views.RoomsList.Character.ICharacterViewModelBuilder;

namespace DM.Web.Classic.Controllers.CharacterControllers
{
    public class CharacterController : DmControllerBase
    {
        private readonly IRoomClaimsCreatingService claimsCreatingService;
        private readonly IRoomClaimsDeletingService claimsDeletingService;
        private readonly ICharacterDeletingService characterDeletingService;
        private readonly CharacterViewModelBuilder characterViewModelBuilder;
        private readonly ListCharacterViewModelBuilder listCharacterViewModelBuilder;

        public CharacterController(
            IRoomClaimsCreatingService claimsCreatingService,
            IRoomClaimsDeletingService claimsDeletingService,
            ICharacterDeletingService characterDeletingService,
            CharacterViewModelBuilder characterViewModelBuilder,
            ListCharacterViewModelBuilder listCharacterViewModelBuilder)
        {
            this.claimsCreatingService = claimsCreatingService;
            this.claimsDeletingService = claimsDeletingService;
            this.characterDeletingService = characterDeletingService;
            this.characterViewModelBuilder = characterViewModelBuilder;
            this.listCharacterViewModelBuilder = listCharacterViewModelBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid characterId)
        {
            var characterViewModel = await characterViewModelBuilder.Build(characterId);
            return PartialView("Character", characterViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AttachToRoom(Guid characterId, Guid roomId)
        {
            var claim = await claimsCreatingService.Create(new CreateRoomClaim
            {
                RoomId = roomId,
                CharacterId = characterId,
                Policy = RoomAccessPolicy.Full
            });
            ViewData["RoomId"] = roomId;
            var characterViewModel = listCharacterViewModelBuilder.Build(claim.Character);
            return PartialView("~/Views/RoomsList/Room/Character/Character.cshtml", characterViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DetachFromRoom(Guid claimId)
        {
            await claimsDeletingService.Delete(claimId);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid characterId)
        {
            await characterDeletingService.Delete(characterId);
            return NoContent();
        }
    }
}