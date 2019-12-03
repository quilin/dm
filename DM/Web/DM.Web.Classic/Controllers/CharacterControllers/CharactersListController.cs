using System;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.BusinessProcesses.Characters.Reading;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Web.Classic.Views.CharactersList;
using DM.Web.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.CharacterControllers
{
    public class CharactersListController : DmControllerBase
    {
        private readonly ICharacterReadingService characterService;
        private readonly IGameReadingService gameService;
        private readonly ICharactersListViewModelBuilder charactersListViewModelBuilder;

        public CharactersListController(
            ICharacterReadingService characterService,
            IGameReadingService gameService,
            ICharactersListViewModelBuilder charactersListViewModelBuilder)
        {
            this.characterService = characterService;
            this.gameService = gameService;
            this.charactersListViewModelBuilder = charactersListViewModelBuilder;
        }

        public async Task<IActionResult> Index(Guid? gameId = null, Guid? characterId = null)
        {
            switch (gameId)
            {
                case null when !characterId.HasValue:
                    throw new HttpException(HttpStatusCode.BadRequest, "Wrong parameters");
                case null:
                    var character = await characterService.GetCharacter(characterId.Value);
                    var game = await gameService.GetGame(character.GameId);
                    return RedirectToAction("Index", new RouteValueDictionary
                    {
                        {"moduleId", game.Id.EncodeToReadable(game.Title)},
                        {"characterId", character.Id.EncodeToReadable(character.Name)}
                    });
                default:
                    var charactersListViewModel = await charactersListViewModelBuilder.Build(gameId.Value, characterId);
                    return View("CharactersList", charactersListViewModel);
            }
        }
    }
}