using System.Linq;
using System.Threading.Tasks;
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
        public Task<bool> IsAllowed(AuthenticatedUser user, CharacterIntention intention,
            CharacterToUpdate target)
        {
            var characterOwned = target.UserId == user.UserId;
            var gameOwned = target.GameMasterId == user.UserId || target.GameAssistantId == user.UserId;
            var gameActive = target.GameStatus == GameStatus.Active || target.GameStatus == GameStatus.Requirement;

            switch (intention)
            {
                case CharacterIntention.Edit when characterOwned:
                    return Task.FromResult(gameActive);
                case CharacterIntention.Edit when gameOwned:
                    return Task.FromResult(target.IsNpc ||
                        target.AccessPolicy.HasFlag(CharacterAccessPolicy.EditAllowed));
                case CharacterIntention.EditPrivacySettings when characterOwned:
                    return Task.FromResult(gameActive);
                case CharacterIntention.EditMasterSettings when gameOwned:
                    return Task.FromResult(true);
                case CharacterIntention.Delete when characterOwned:
                    return Task.FromResult(gameActive);
                case CharacterIntention.Accept when gameOwned:
                    return Task.FromResult(target.Status == CharacterStatus.Registration ||
                        target.Status == CharacterStatus.Declined);
                case CharacterIntention.Decline when gameOwned:
                    return Task.FromResult(target.Status == CharacterStatus.Registration);
                case CharacterIntention.Kill when gameOwned:
                    return Task.FromResult(target.Status == CharacterStatus.Active);
                case CharacterIntention.Resurrect when gameOwned:
                    return Task.FromResult(target.Status == CharacterStatus.Dead);
                case CharacterIntention.Leave when characterOwned:
                    return Task.FromResult(target.Status == CharacterStatus.Active);
                case CharacterIntention.Return when characterOwned:
                    return Task.FromResult(target.Status == CharacterStatus.Left);
                default:
                    return Task.FromResult(false);
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
        public Task<bool> IsAllowed(AuthenticatedUser user, CharacterIntention intention, (Character, GameExtended) target)
        {
            var (character, game) = target;
            
            if (!CharacterGameIntentions.Contains(intention))
            {
                return Task.FromResult(false);
            }

            if (game.Participation(user.UserId).HasFlag(GameParticipation.Authority))
            {
                return Task.FromResult(true);
            }

            if (character.Author.UserId == user.UserId)
            {
                return Task.FromResult(true);
            }

            switch (intention)
            {
                case CharacterIntention.ViewTemper when !game.HideTemper:
                case CharacterIntention.ViewStory when !game.HideStory:
                case CharacterIntention.ViewSkills when !game.HideSkills:
                case CharacterIntention.ViewInventory when !game.HideInventory:
                    return Task.FromResult(true);
                default:
                    return Task.FromResult(false);
            }
        }
    }
}