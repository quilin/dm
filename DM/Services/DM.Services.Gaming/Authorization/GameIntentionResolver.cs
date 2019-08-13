using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
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

        /// <inheritdoc />
        public Task<bool> IsAllowed(AuthenticatedUser user, GameIntention intention, GameExtended target)
        {
            return Task.FromResult(false);
        }
    }
}