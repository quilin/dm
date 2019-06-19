using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Forum.Dto.Output;

namespace DM.Services.Forum.Authorization
{
    /// <inheritdoc />
    public class TopicIntentionResolver : IIntentionResolver<TopicIntention, Topic>
    {
        /// <inheritdoc />
        public Task<bool> IsAllowed(AuthenticatedUser user, TopicIntention intention, Topic target)
        {
            switch (intention)
            {
                case TopicIntention.CreateComment when user.IsAuthenticated:
                    return Task.FromResult(!target.Closed);
                case TopicIntention.Edit when user.IsAuthenticated:
                    return Task.FromResult(target.Author.UserId == user.UserId && !target.Closed ||
                                           target.Forum.ModeratorIds.Contains(user.UserId) ||
                                           user.Role.HasFlag(UserRole.Administrator));
                case TopicIntention.Like when user.IsAuthenticated:
                    return Task.FromResult(target.Author.UserId != user.UserId);
                default:
                    return Task.FromResult(false);
            }
        }
    }
}