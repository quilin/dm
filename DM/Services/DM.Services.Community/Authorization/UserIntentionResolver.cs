using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto;

namespace DM.Services.Community.Authorization
{
    /// <inheritdoc />
    public class UserIntentionResolver : IIntentionResolver<UserIntention, GeneralUser>
    {
        /// <inheritdoc />
        public bool IsAllowed(AuthenticatedUser user, UserIntention intention, GeneralUser target)
        {
            switch (intention)
            {
                case UserIntention.Edit:
                    return target.UserId == user.UserId;
                default:
                    return false;
            }
        }
    }
}