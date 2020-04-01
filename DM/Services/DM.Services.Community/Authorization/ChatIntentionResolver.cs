using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;

namespace DM.Services.Community.Authorization
{
    /// <inheritdoc />
    public class ChatIntentionResolver : IIntentionResolver<ChatIntention>
    {
        /// <inheritdoc />
        public bool IsAllowed(AuthenticatedUser user, ChatIntention intention) => intention switch
        {
            ChatIntention.CreateMessage => user.IsAuthenticated,
            _ => false
        };
    }
}