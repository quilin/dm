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
                    return (CharacterIntention.Decline, EventType.Unknown);
                case CharacterStatus.Active when statusFrom == CharacterStatus.Registration:
                case CharacterStatus.Active when statusFrom == CharacterStatus.Declined:
                    return (CharacterIntention.Accept, EventType.Unknown);
                case CharacterStatus.Active when statusFrom == CharacterStatus.Dead:
                    return (CharacterIntention.Resurrect, EventType.Unknown);
                case CharacterStatus.Active when statusFrom == CharacterStatus.Left:
                    return (CharacterIntention.Return, EventType.Unknown);
                case CharacterStatus.Dead:
                    return (CharacterIntention.Kill, EventType.Unknown);
                case CharacterStatus.Left:
                    return (CharacterIntention.Leave, EventType.Unknown);
                default:
                    throw new CharacterIntentionConverterException(statusTo);
            }
        }
    }
}