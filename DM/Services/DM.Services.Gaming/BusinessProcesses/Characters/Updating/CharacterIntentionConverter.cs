using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Authorization;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Updating
{
    /// <inheritdoc />
    public class CharacterIntentionConverter : ICharacterIntentionConverter
    {
        /// <inheritdoc />
        public (CharacterIntention intention, EventType eventType) Convert(
            CharacterStatus statusFrom, CharacterStatus statusTo)
        {
            switch (statusTo)
            {
                case CharacterStatus.Declined:
                    return (CharacterIntention.Decline, EventType.StatusCharacterDeclined);
                case CharacterStatus.Active when statusFrom == CharacterStatus.Registration:
                case CharacterStatus.Active when statusFrom == CharacterStatus.Declined:
                    return (CharacterIntention.Accept, EventType.StatusCharacterAccepted);
                case CharacterStatus.Active when statusFrom == CharacterStatus.Dead:
                    return (CharacterIntention.Resurrect, EventType.StatusCharacterResurrected);
                case CharacterStatus.Active when statusFrom == CharacterStatus.Left:
                    return (CharacterIntention.Return, EventType.StatusCharacterReturned);
                case CharacterStatus.Dead:
                    return (CharacterIntention.Kill, EventType.StatusCharacterDied);
                case CharacterStatus.Left:
                    return (CharacterIntention.Leave, EventType.StatusCharacterLeft);
                default:
                    throw new CharacterIntentionConverterException(statusTo);
            }
        }
    }
}