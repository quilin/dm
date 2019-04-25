using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Implementation;
using DM.Services.Forum.Dto.Output;

namespace DM.Services.Forum.Authorization
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
                    return Task.FromResult(target.Author.UserId == user.UserId);
                case CommentIntention.Like when user.IsAuthenticated:
                    return Task.FromResult(target.Author.UserId != user.UserId);
                default:
                    return Task.FromResult(false);
            }
        }
    }
}