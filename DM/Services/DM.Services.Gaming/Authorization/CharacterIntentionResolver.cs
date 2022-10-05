using System.Linq;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Authorization;

/// <inheritdoc cref="IIntentionResolver" />
internal class CharacterIntentionResolver :
    IIntentionResolver<CharacterIntention, CharacterToUpdate>,
    IIntentionResolver<CharacterIntention, (Character, GameExtended)>
{
    /// <inheritdoc />
    public bool IsAllowed(AuthenticatedUser user, CharacterIntention intention,
        CharacterToUpdate target)
    {
        var characterOwned = target.UserId == user.UserId;
        var gameOwned = target.GameMasterId == user.UserId || target.GameAssistantId == user.UserId;
        var gameActive = target.GameStatus == GameStatus.Active || target.GameStatus == GameStatus.Requirement;

        return intention switch
        {
            CharacterIntention.Edit when characterOwned => gameActive,
            CharacterIntention.Edit when gameOwned => target.IsNpc ||
                                                      target.AccessPolicy.HasFlag(CharacterAccessPolicy.EditAllowed),
            CharacterIntention.EditPrivacySettings when characterOwned => gameActive,
            CharacterIntention.EditMasterSettings when gameOwned => true,
            CharacterIntention.Delete when characterOwned => gameActive,
            CharacterIntention.Accept when gameOwned => target.Status == CharacterStatus.Registration ||
                                                        target.Status == CharacterStatus.Declined,
            CharacterIntention.Decline when gameOwned => target.Status == CharacterStatus.Registration,
            CharacterIntention.Kill when gameOwned => target.Status == CharacterStatus.Active,
            CharacterIntention.Resurrect when gameOwned => target.Status == CharacterStatus.Dead,
            CharacterIntention.Leave when characterOwned => target.Status == CharacterStatus.Active,
            CharacterIntention.Return when characterOwned => target.Status == CharacterStatus.Left,
            _ => false
        };
    }

    private static readonly CharacterIntention[] CharacterGameIntentions =
    {
        CharacterIntention.ViewTemper,
        CharacterIntention.ViewStory,
        CharacterIntention.ViewSkills,
        CharacterIntention.ViewInventory
    };

    /// <inheritdoc />
    public bool IsAllowed(AuthenticatedUser user, CharacterIntention intention, (Character, GameExtended) target)
    {
        var (character, game) = target;
            
        if (!CharacterGameIntentions.Contains(intention))
        {
            return false;
        }

        if (game.Participation(user.UserId).HasFlag(GameParticipation.Authority))
        {
            return true;
        }

        if (character.Author.UserId == user.UserId)
        {
            return true;
        }

        return intention switch
        {
            CharacterIntention.ViewTemper when !game.HideTemper => true,
            CharacterIntention.ViewStory when !game.HideStory => true,
            CharacterIntention.ViewSkills when !game.HideSkills => true,
            CharacterIntention.ViewInventory when !game.HideInventory => true,
            _ => false
        };
    }
}