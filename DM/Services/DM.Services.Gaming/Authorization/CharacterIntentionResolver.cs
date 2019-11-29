using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto.Internal;

namespace DM.Services.Gaming.Authorization
{
    /// <inheritdoc />
    public class CharacterIntentionResolver : IIntentionResolver<CharacterIntention, CharacterToUpdate>
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
    }
}