using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Forum.Authorization
{
    /// <inheritdoc />
    public class CommentIntentionResolver : IIntentionResolver<CommentIntention, Comment>
    {
        /// <inheritdoc />
        public bool IsAllowed(AuthenticatedUser user, CommentIntention intention, Comment target)
        {
            switch (intention)
            {
                case CommentIntention.Edit when user.IsAuthenticated:
                case CommentIntention.Delete when user.IsAuthenticated:
                    return user.Role.HasFlag(UserRole.Administrator) ||
                        target.Author.UserId == user.UserId;
                case CommentIntention.Like when user.IsAuthenticated:
                    return target.Author.UserId != user.UserId;
                default:
                    return false;
            }
        }
    }
}