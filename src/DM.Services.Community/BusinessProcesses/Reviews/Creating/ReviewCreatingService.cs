using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Reviews.Creating;

/// <inheritdoc />
internal class ReviewCreatingService(
    IValidator<CreateReview> validator,
    IIntentionManager intentionManager,
    IReviewFactory factory,
    IReviewCreatingRepository repository,
    IIdentityProvider identityProvider) : IReviewCreatingService
{
    /// <inheritdoc />
    public async Task<Review> Create(CreateReview createReview, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createReview, cancellationToken);
        intentionManager.ThrowIfForbidden(ReviewIntention.Create);

        var review = factory.Create(createReview, identityProvider.Current.User.UserId);
        return await repository.Create(review, cancellationToken);
    }
}