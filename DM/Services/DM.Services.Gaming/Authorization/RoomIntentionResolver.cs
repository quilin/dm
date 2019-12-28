using System;
using System.Linq;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Dto;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Authorization
{
    /// <inheritdoc />
    public class RoomIntentionResolver :
        IIntentionResolver<RoomIntention, (RoomToUpdate, Guid?)>,
        IIntentionResolver<RoomIntention, RoomToUpdate>,
        IIntentionResolver<RoomIntention, PendingPost>
    {
        /// <inheritdoc />
        public bool IsAllowed(AuthenticatedUser user, RoomIntention intention, (RoomToUpdate, Guid?) target)
        {
            var (room, characterId) = target;

            switch (intention)
            {
                case RoomIntention.CreatePost when characterId.HasValue:
                    var claim = room.Claims.FirstOrDefault(c => c.Character?.Id == characterId.Value);
                    return claim != null &&
                        (claim.Character.Author.UserId == user.UserId ||
                            claim.Character.IsNpc &&
                            room.Game.Participation(user.UserId).HasFlag(GameParticipation.Authority));
                case RoomIntention.CreatePost:
                    return room.Game.Participation(user.UserId).HasFlag(GameParticipation.Authority);
                default:
                    return false;
            }
        }

        /// <inheritdoc />
        public bool IsAllowed(AuthenticatedUser user, RoomIntention intention, RoomToUpdate target)
        {
            switch (intention)
            {
                case RoomIntention.CreatePendingPost:
                    return target.Claims.Any(c => c.Character?.Author.UserId == user.UserId) ||
                        target.Game.Participation(user.UserId).HasFlag(GameParticipation.Authority);
                default:
                    return false;
            }
        }

        /// <inheritdoc />
        public bool IsAllowed(AuthenticatedUser user, RoomIntention intention, PendingPost target)
        {
            switch (intention)
            {
                case RoomIntention.DeletePending:
                    return target.AwaitingUser.UserId == user.UserId;
                default:
                    return false;
            }
        }
    }
}