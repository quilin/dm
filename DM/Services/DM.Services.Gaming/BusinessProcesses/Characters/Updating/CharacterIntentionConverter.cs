using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Authorization;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Updating;

/// <inheritdoc />
internal class CharacterIntentionConverter : ICharacterIntentionConverter
{
    /// <inheritdoc />
    public (CharacterIntention intention, EventType eventType) Convert(
        CharacterStatus statusFrom, CharacterStatus statusTo) => statusTo switch
    {
        CharacterStatus.Declined => (CharacterIntention.Decline, EventType.StatusCharacterDeclined),
        CharacterStatus.Active when statusFrom == CharacterStatus.Registration => (CharacterIntention.Accept,
            EventType.StatusCharacterAccepted),
        CharacterStatus.Active when statusFrom == CharacterStatus.Declined => (CharacterIntention.Accept,
            EventType.StatusCharacterAccepted),
        CharacterStatus.Active when statusFrom == CharacterStatus.Dead => (CharacterIntention.Resurrect,
            EventType.StatusCharacterResurrected),
        CharacterStatus.Active when statusFrom == CharacterStatus.Left => (CharacterIntention.Return,
            EventType.StatusCharacterReturned),
        CharacterStatus.Dead => (CharacterIntention.Kill, EventType.StatusCharacterDied),
        CharacterStatus.Left => (CharacterIntention.Leave, EventType.StatusCharacterLeft),
        _ => throw new CharacterIntentionConverterException(statusTo)
    };
}