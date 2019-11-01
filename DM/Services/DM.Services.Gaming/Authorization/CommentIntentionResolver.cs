using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.Authorization
{
    /// <inheritdoc />
    public class CommentIntentionResolver : IIntentionResolver<CommentIntention, Comment>
    {
        /// <inheritdoc />
        public Task<bool> IsAllowed(AuthenticatedUser user, CommentIntention intention, Comment target)
        {
            switch (intention)
            {
                case CommentIntention.Edit when user.IsAuthenticated:
                case CommentIntention.Delete when user.IsAuthenticated:
                    return Task.FromResult(
                        user.Role.HasFlag(UserRole.Administrator) ||
                        target.Author.UserId == user.UserId);
                case CommentIntention.Like when user.IsAuthenticated:
                    return Task.FromResult(target.Author.UserId != user.UserId);
                default:
                    return Task.FromResult(false);
            }
        }
    }
}