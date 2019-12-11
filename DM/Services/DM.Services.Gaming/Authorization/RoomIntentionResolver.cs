using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Authorization
{
    /// <inheritdoc />
    public class RoomIntentionResolver : IIntentionResolver<RoomIntention, Room>
    {
        /// <inheritdoc />
        public Task<bool> IsAllowed(AuthenticatedUser user, RoomIntention intention, Room target)
        {
            switch (intention)
            {
                case RoomIntention.CreatePost when user.IsAuthenticated:
                    return Task.FromResult(target.Claims.Any(c => c.Character.Author.UserId == user.UserId));
                case RoomIntention.CreatePendingPost when user.IsAuthenticated:
                    return Task.FromResult(
                        target.Claims.Any(c => c.Character.Author.UserId == user.UserId) &&
                        target.Pendings.All(p => p.AwaitingUser.UserId != user.UserId));
                default:
                    throw new ArgumentOutOfRangeException(nameof(intention), intention, null);
            }
        }
    }
}