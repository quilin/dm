using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Authorization
{
    /// <inheritdoc />
    public class PostIntentionResolver :
        IIntentionResolver<PostIntention, Post>,
        IIntentionResolver<PostIntention, (Post, RoomToUpdate)>
    {
        /// <inheritdoc />
        public bool IsAllowed(AuthenticatedUser user, PostIntention intention, Post target)
        {
            switch (intention)
            {
                case PostIntention.Delete:
                    return target.Author.UserId == user.UserId;
                default:
                    return false;
            }
        }

        /// <inheritdoc />
        public bool IsAllowed(AuthenticatedUser user, PostIntention intention, (Post, RoomToUpdate) target)
        {
            var (post, room) = target;
            switch (intention)
            {
                case PostIntention.EditText:
                    return post.Author.UserId == user.UserId ||
                        room.Game.Participation(user.UserId).HasFlag(GameParticipation.Authority) && (
                            post.Character == null ||
                            post.Character.IsNpc ||
                            post.Character.AccessPolicy.HasFlag(CharacterAccessPolicy.PostEditAllowed));
                case PostIntention.EditCharacter:
                    return post.Author.UserId == user.UserId ||
                        room.Game.Participation(user.UserId).HasFlag(GameParticipation.Authority) && (
                            post.Character == null ||
                            post.Character.IsNpc);
                case PostIntention.EditMasterMessage:
                    return room.Game.Participation(user.UserId).HasFlag(GameParticipation.Authority) && (
                        post.Character == null ||
                        post.Character.IsNpc);
                default:
                    return false;
            }
        }
    }
}