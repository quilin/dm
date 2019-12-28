using System.Linq;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Authorization
{
    /// <inheritdoc cref="IIntentionResolver" />
    public class CharacterIntentionResolver :
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

            switch (intention)
            {
                case CharacterIntention.Edit when characterOwned:
                    return gameActive;
                case CharacterIntention.Edit when gameOwned:
                    return target.IsNpc ||
                        target.AccessPolicy.HasFlag(CharacterAccessPolicy.EditAllowed);
                case CharacterIntention.EditPrivacySettings when characterOwned:
                    return gameActive;
                case CharacterIntention.EditMasterSettings when gameOwned:
                    return true;
                case CharacterIntention.Delete when characterOwned:
                    return gameActive;
                case CharacterIntention.Accept when gameOwned:
                    return target.Status == CharacterStatus.Registration ||
                        target.Status == CharacterStatus.Declined;
                case CharacterIntention.Decline when gameOwned:
                    return target.Status == CharacterStatus.Registration;
                case CharacterIntention.Kill when gameOwned:
                    return target.Status == CharacterStatus.Active;
                case CharacterIntention.Resurrect when gameOwned:
                    return target.Status == CharacterStatus.Dead;
                case CharacterIntention.Leave when characterOwned:
                    return target.Status == CharacterStatus.Active;
                case CharacterIntention.Return when characterOwned:
                    return target.Status == CharacterStatus.Left;
                default:
                    return false;
            }
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

            switch (intention)
            {
                case CharacterIntention.ViewTemper when !game.HideTemper:
                case CharacterIntention.ViewStory when !game.HideStory:
                case CharacterIntention.ViewSkills when !game.HideSkills:
                case CharacterIntention.ViewInventory when !game.HideInventory:
                    return true;
                default:
                    return false;
            }
        }
    }
}