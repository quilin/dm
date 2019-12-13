using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Authorization
{
    /// <inheritdoc />
    public class RoomIntentionResolver :
        IIntentionResolver<RoomIntention, Room>,
        IIntentionResolver<RoomIntention, PendingPost>,
        IIntentionResolver<RoomIntention, Room, CreatePost>
    {
        /// <inheritdoc />
        public Task<bool> IsAllowed(AuthenticatedUser user, RoomIntention intention, Room target)
        {
            switch (intention)
            {
                case RoomIntention.CreatePost when user.IsAuthenticated:
                case RoomIntention.CreatePendingPost when user.IsAuthenticated:
                    return Task.FromResult(target.Claims.Any(c => c.Character.Author.UserId == user.UserId));
                default:
                    return Task.FromResult(false);
            }
        }

        /// <inheritdoc />
        public Task<bool> IsAllowed(AuthenticatedUser user, RoomIntention intention, PendingPost target)
        {
            switch (intention)
            {
                case RoomIntention.DeletePending:
                    return Task.FromResult(target.AwaitingUser.UserId == user.UserId);
                default:
                    return Task.FromResult(false);
            }
        }

        /// <inheritdoc />
        public Task<bool> IsAllowed(AuthenticatedUser user, RoomIntention intention, Room target, CreatePost subTarget)
        {
            switch (intention)
            {
                case RoomIntention.CreatePost:
                    return Task.FromResult(target.Claims.Any(c =>
                        c.Character.Author.UserId == user.UserId &&
                        c.Character.Id == subTarget.CharacterId));
                default:
                    return Task.FromResult(false);
            }
        }
    }
}