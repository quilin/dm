using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;

namespace DM.Services.Community.Authorization
{
    /// <inheritdoc />
    public class ReviewIntentionResolver :
        IIntentionResolver<ReviewIntention>,
        IIntentionResolver<ReviewIntention, Review>
    {
        /// <inheritdoc />
        public bool IsAllowed(AuthenticatedUser user, ReviewIntention intention)
        {
            switch (intention)
            {
                case ReviewIntention.Create:
                    return user.QuantityRating > 1000;
                default:
                    return false;
            }
        }

        /// <inheritdoc />
        public bool IsAllowed(AuthenticatedUser user, ReviewIntention intention, Review target)
        {
            switch (intention)
            {
                default:
                    return false;
            }
        }
    }
}