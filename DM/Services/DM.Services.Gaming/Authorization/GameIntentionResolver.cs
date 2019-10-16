using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Authorization
{
    /// <inheritdoc cref="IIntentionResolver" />
    public class GameIntentionResolver :
        IIntentionResolver<GameIntention>,
        IIntentionResolver<GameIntention, GameExtended>
    {
        /// <inheritdoc />
        public Task<bool> IsAllowed(AuthenticatedUser user, GameIntention intention)
        {
            switch (intention)
            {
                case GameIntention.Create when user.IsAuthenticated:
                    return Task.FromResult(true);
                default:
                    return Task.FromResult(false);
            }
        }

        private static readonly IEnumerable<GameStatus> HiddenStates = new HashSet<GameStatus>
        {
            GameStatus.Draft,
            GameStatus.RequiresModeration,
            GameStatus.Moderation
        };

        /// <inheritdoc />
        public Task<bool> IsAllowed(AuthenticatedUser user, GameIntention intention, GameExtended target)
        {
            switch (intention)
            {
                case GameIntention.Read:
                    return Task.FromResult(user.Role.HasFlag(UserRole.Administrator | UserRole.SeniorModerator) ||
                                           user.UserId == target.Master.UserId ||
                                           user.UserId == target.Assistant?.UserId ||
                                           !HiddenStates.Contains(target.Status));
                default:
                    return Task.FromResult(false);
            }
        }
    }
}