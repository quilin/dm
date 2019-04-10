using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Dto;
using DM.Services.Common.Implementation;

namespace DM.Services.Common.Authorization
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
                default:
                    return Task.FromResult(false);
            }
        }
    }
}