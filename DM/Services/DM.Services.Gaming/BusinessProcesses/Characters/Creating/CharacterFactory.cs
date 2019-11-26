using System;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Creating
{
    /// <inheritdoc />
    public class CharacterFactory : ICharacterFactory
    {
        private readonly IGuidFactory guidFactory;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <inheritdoc />
        public CharacterFactory(
            IGuidFactory guidFactory,
            IDateTimeProvider dateTimeProvider)
        {
            this.guidFactory = guidFactory;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <inheritdoc />
        public Character Create(CreateCharacter createCharacter, Guid userId, CharacterStatus initialStatus)
        {
            return new Character
            {
                CharacterId = guidFactory.Create(),
                CreateDate = dateTimeProvider.Now,
                GameId = createCharacter.GameId,
                UserId = userId,
                Name = createCharacter.Name,
                Race = createCharacter.Race,
                Class = createCharacter.Class,
                Alignment = createCharacter.Alignment,
                Appearance = createCharacter.Appearance,
                Temper = createCharacter.Temper,
                Story = createCharacter.Story,
                Skills = createCharacter.Skills,
                Inventory = createCharacter.Inventory,
                IsNpc = createCharacter.IsNpc,
                AccessPolicy = createCharacter.AccessPolicy
            };
        }
    }
}