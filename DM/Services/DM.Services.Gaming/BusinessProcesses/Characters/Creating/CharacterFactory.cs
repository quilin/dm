using System;
using System.Collections.Generic;
using System.Linq;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Creating;

/// <inheritdoc />
internal class CharacterFactory : ICharacterFactory
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
    public (Character, IEnumerable<CharacterAttribute>) Create(CreateCharacter createCharacter, Guid userId,
        CharacterStatus initialStatus)
    {
        var characterId = guidFactory.Create();
        var character = new Character
        {
            CharacterId = characterId,
            CreateDate = dateTimeProvider.Now,
            GameId = createCharacter.GameId,
            UserId = userId,
            Status = initialStatus,
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
        var attributes = createCharacter.Attributes?.Select(a => new CharacterAttribute
        {
            CharacterAttributeId = guidFactory.Create(),
            CharacterId = characterId,
            AttributeId = a.Id,
            Value = a.Value.Trim()
        }) ?? Enumerable.Empty<CharacterAttribute>();
        return (character, attributes);
    }
}