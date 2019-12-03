using System;
using System.Collections.Generic;
using DM.Services.Gaming.Dto.Output;
using ViewModel = DM.Web.Classic.Views.CharactersList.CharactersSublistViewModel;
using DtoCharacter = DM.Services.Gaming.Dto.Output.Character;

namespace DM.Web.Classic.Views.CharactersList
{
    public interface ICharactersSublistViewModelBuilder
    {
        ViewModel BuildPlayer(IEnumerable<DtoCharacter> characters, GameExtended game, Guid? defaultCharacterId);
        ViewModel BuildNpc(IEnumerable<DtoCharacter> characters, GameExtended game, Guid? defaultCharacterId);
        ViewModel BuildOutOfGame(IEnumerable<DtoCharacter> characters, GameExtended game, Guid? defaultCharacterId);
        ViewModel BuildDeclined(IEnumerable<DtoCharacter> characters, GameExtended game, Guid? defaultCharacterId);
    }
}