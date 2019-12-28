using System;
using System.Collections.Generic;
using System.Linq;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Views.CharactersList.Character;
using ViewModel = DM.Web.Classic.Views.CharactersList.CharactersSublistViewModel;
using DtoCharacter = DM.Services.Gaming.Dto.Output.Character;

namespace DM.Web.Classic.Views.CharactersList
{
    public class CharactersSublistViewModelBuilder : ICharactersSublistViewModelBuilder
    {
        private readonly ICharacterViewModelBuilder characterViewModelBuilder;

        public CharactersSublistViewModelBuilder(
            ICharacterViewModelBuilder characterViewModelBuilder)
        {
            this.characterViewModelBuilder = characterViewModelBuilder;
        }

        private ViewModel Build(GameExtended game, Guid? defaultCharacterId, DtoCharacter[] characters)
        {
            return new CharactersSublistViewModel
            {
                Characters = characters.Select(c => characterViewModelBuilder.Build(c, game).Result).ToArray(),
                IsDefault = defaultCharacterId.HasValue && characters.Any(c => c.Id == defaultCharacterId.Value)
            };
        }

        public ViewModel BuildPlayer(IEnumerable<DtoCharacter> characters, GameExtended game, Guid? defaultCharacterId)
        {
            var charactersSublistViewModel = Build(game, defaultCharacterId, characters
                .Where(c => !c.IsNpc &&
                    (c.Status == CharacterStatus.Active || c.Status == CharacterStatus.Registration))
                .ToArray());
            charactersSublistViewModel.Id = "Players";
            charactersSublistViewModel.Title = "В игре";
            return charactersSublistViewModel;
        }

        public ViewModel BuildNpc(IEnumerable<DtoCharacter> characters, GameExtended game, Guid? defaultCharacterId)
        {
            var charactersSublistViewModel = Build(game, defaultCharacterId, characters
                .Where(c => c.IsNpc)
                .ToArray());
            charactersSublistViewModel.Id = "Npc";
            charactersSublistViewModel.Title = "Мастерские";
            return charactersSublistViewModel;
        }

        public ViewModel BuildOutOfGame(IEnumerable<DtoCharacter> characters, GameExtended game, Guid? defaultCharacterId)
        {
            var charactersSublistViewModel = Build(game, defaultCharacterId, characters
                .Where(c => !c.IsNpc &&
                        (c.Status == CharacterStatus.Dead || c.Status == CharacterStatus.Left))
                .ToArray());
            charactersSublistViewModel.Id = "Dead";
            charactersSublistViewModel.Title = "Вне игры";
            return charactersSublistViewModel;
        }

        public ViewModel BuildDeclined(IEnumerable<DtoCharacter> characters, GameExtended game, Guid? defaultCharacterId)
        {
            var charactersSublistViewModel = Build(game, defaultCharacterId, characters
                .Where(c => !c.IsNpc && c.Status == CharacterStatus.Declined)
                .ToArray());
            charactersSublistViewModel.Id = "Declined";
            charactersSublistViewModel.Title = "Отклонённые";
            return charactersSublistViewModel;
        }
    }
}