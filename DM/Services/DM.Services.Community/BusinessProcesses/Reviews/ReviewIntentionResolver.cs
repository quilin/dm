using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Community.BusinessProcesses.Reviews;

/// <inheritdoc />
internal class ReviewIntentionResolver :
    IIntentionResolver<ReviewIntention>,
    IIntentionResolver<ReviewIntention, Review>
{
    /// <inheritdoc />
    public bool IsAllowed(AuthenticatedUser user, ReviewIntention intention) => intention switch
    {
        ReviewIntention.Create => user.QuantityRating >= 1000,
        ReviewIntention.ReadUnapproved => user.Role.HasFlag(UserRole.Administrator),
        _ => false
    };

    /// <inheritdoc />
    public bool IsAllowed(AuthenticatedUser user, ReviewIntention intention, Review target) => intention switch
    {
        ReviewIntention.Edit => !target.Approved && user.UserId == target.Author.UserId,
        ReviewIntention.Approve => !target.Approved && user.Role.HasFlag(UserRole.Administrator),
        ReviewIntention.Delete => user.Role.HasFlag(UserRole.Administrator),
        _ => false
    };
}