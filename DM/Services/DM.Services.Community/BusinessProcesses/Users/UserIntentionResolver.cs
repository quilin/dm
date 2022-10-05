using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Users;

/// <inheritdoc />
internal class UserIntentionResolver : IIntentionResolver<UserIntention, GeneralUser>
{
    /// <inheritdoc />
    public bool IsAllowed(AuthenticatedUser user, UserIntention intention, GeneralUser target) => intention switch
    {
        UserIntention.Edit => target.UserId == user.UserId,
        UserIntention.WriteMessage => target.UserId != user.UserId,
        _ => false
    };
}