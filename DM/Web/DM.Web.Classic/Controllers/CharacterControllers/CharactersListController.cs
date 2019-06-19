using System;
using System.Net;
using DM.Web.Classic.Views.CharactersList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.CharacterControllers
{
    public class CharactersListController : DmControllerBase
    {
        private readonly ICharactersListViewModelBuilder charactersListViewModelBuilder;
        private readonly ICharacterService characterService;
        private readonly IModuleService moduleService;

        public CharactersListController(
            ICharactersListViewModelBuilder charactersListViewModelBuilder,
            ICharacterService characterService,
            IModuleService moduleService
        )
        {
            this.charactersListViewModelBuilder = charactersListViewModelBuilder;
            this.characterService = characterService;
            this.moduleService = moduleService;
        }

        public IActionResult Index(Guid? moduleId = null, Guid? characterId = null)
        {
            switch (moduleId)
            {
                case null when !characterId.HasValue:
                    throw new HttpException(HttpStatusCode.BadRequest, "Wrong parameters");
                case null:
                    var character = characterService.Read(characterId.Value);
                    var module = moduleService.Read(character.ModuleId);
                    return RedirectToAction("Index", new RouteValueDictionary
                    {
                        {"moduleId", module.ModuleId.EncodeToReadable(module.Title)},
                        {"characterId", character.CharacterId.EncodeToReadable(character.Name)}
                    });
                default:
                    var charactersListViewModel = charactersListViewModelBuilder.Build(moduleId.Value, characterId);
                    return View("CharactersList", charactersListViewModel);
            }
        }
    }
}