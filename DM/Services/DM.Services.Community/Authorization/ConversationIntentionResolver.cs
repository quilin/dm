using System.Linq;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;

namespace DM.Services.Community.Authorization
{
    /// <inheritdoc />
    public class ConversationIntentionResolver : IIntentionResolver<ConversationIntention, Conversation>
    {
        /// <inheritdoc />
        public bool IsAllowed(AuthenticatedUser user, ConversationIntention intention, Conversation target) =>
            intention switch
            {
                ConversationIntention.CreateMessage => target.Participants.Any(p => p.UserId == user.UserId),
                _ => false
            };
    }
}