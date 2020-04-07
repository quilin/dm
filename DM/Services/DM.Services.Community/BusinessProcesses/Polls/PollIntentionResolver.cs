using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Polls.Reading;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;

namespace DM.Services.Community.BusinessProcesses.Polls
{
    /// <inheritdoc />
    public class PollIntentionResolver :
        IIntentionResolver<PollIntention>,
        IIntentionResolver<PollIntention, Poll>
    {
        private readonly IDateTimeProvider dateTimeProvider;

        /// <inheritdoc />
        public PollIntentionResolver(
            IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }
        
        /// <inheritdoc />
        public bool IsAllowed(AuthenticatedUser user, PollIntention intention) => intention switch
        {
            PollIntention.Create => user.Role.HasFlag(UserRole.Administrator),
            _ => false
        };

        /// <inheritdoc />
        public bool IsAllowed(AuthenticatedUser user, PollIntention intention, Poll target) => intention switch
        {
            PollIntention.Vote => user.IsAuthenticated && target.EndDate > dateTimeProvider.Now,
            _ => false
        };
    }
}