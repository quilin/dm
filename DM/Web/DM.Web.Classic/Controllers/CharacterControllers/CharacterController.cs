using System;
using DM.Web.Classic.Views.RoomsList.Character;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CharacterControllers
{
    public class CharacterController : DmControllerBase
    {
        private readonly ICharacterService characterService;
        private readonly IRoomService roomService;
        private readonly ICharacterViewModelBuilder characterViewModelBuilder;
        private readonly Views.CharactersList.Character.ICharacterViewModelBuilder characterDetailsViewModelBuilder;

        public CharacterController(
            ICharacterService characterService,
            IRoomService roomService,
            ICharacterViewModelBuilder characterViewModelBuilder,
            Views.CharactersList.Character.ICharacterViewModelBuilder characterDetailsViewModelBuilder
            )
        {
            this.characterService = characterService;
            this.roomService = roomService;
            this.characterViewModelBuilder = characterViewModelBuilder;
            this.characterDetailsViewModelBuilder = characterDetailsViewModelBuilder;
        }

        [HttpGet]
        public ActionResult Index(Guid characterId)
        {
            var characterViewModel = characterDetailsViewModelBuilder.Build(characterId);
            return PartialView("Character", characterViewModel);
        }

        [HttpPost]
        public ActionResult AttachToRoom(Guid characterId, Guid roomId)
        {
            roomService.CreateRoomCharacterLinks(characterId, roomId);
            var character = characterService.Read(characterId);
            ViewData["RoomId"] = roomId;
            return PartialView("~/Views/RoomsList/Room/Character/Character.cshtml", characterViewModelBuilder.Build(character));
        }

        [HttpPost]
        public void DetachFromRoom(Guid roomId, Guid characterId)
        {
            roomService.RemoveRoomCharacterLinks(characterId, roomId);
        }

        [HttpPost]
        public void Remove(Guid characterId)
        {
            characterService.Remove(characterId);
        }
    }
}