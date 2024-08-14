using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Authorization;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Updating;

/// <summary>
/// Converter for character status into associated intention
/// </summary>
internal interface ICharacterIntentionConverter
{
    /// <summary>
    /// Convert character status to associated intention
    /// </summary>
    /// <param name="statusFrom">Current status</param>
    /// <param name="statusTo">Desired status</param>
    /// <returns>Authorized intention and invoked event type</returns>
    (CharacterIntention intention, EventType eventType) Convert(
        CharacterStatus statusFrom, CharacterStatus statusTo);
}