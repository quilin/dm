using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Reviews.Creating;

/// <inheritdoc />
internal class ReviewCreatingService : IReviewCreatingService
{
    private readonly IValidator<CreateReview> validator;
    private readonly IIntentionManager intentionManager;
    private readonly IReviewFactory factory;
    private readonly IReviewCreatingRepository repository;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public ReviewCreatingService(
        IValidator<CreateReview> validator,
        IIntentionManager intentionManager,
        IReviewFactory factory,
        IReviewCreatingRepository repository,
        IIdentityProvider identityProvider)
    {
        this.validator = validator;
        this.intentionManager = intentionManager;
        this.factory = factory;
        this.repository = repository;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<Review> Create(CreateReview createReview)
    {
        await validator.ValidateAndThrowAsync(createReview);
        intentionManager.ThrowIfForbidden(ReviewIntention.Create);

        var review = factory.Create(createReview, identityProvider.Current.User.UserId);
        return await repository.Create(review);
    }
}