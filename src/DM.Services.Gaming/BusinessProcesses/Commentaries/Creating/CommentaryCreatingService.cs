using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.Commentaries;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;
using Comment = DM.Services.Common.Dto.Comment;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Creating;

/// <inheritdoc />
internal class CommentaryCreatingService(
    IValidator<CreateComment> validator,
    IGameReadingService gameReadingService,
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    ICommentaryFactory commentaryFactory,
    ICommentaryCreatingRepository repository,
    IUnreadCountersRepository countersRepository,
    IInvokedEventProducer invokedEventProducer) : ICommentaryCreatingService
{
    /// <inheritdoc />
    public async Task<Comment> Create(CreateComment createComment, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createComment, cancellationToken);

        var game = await gameReadingService.GetGame(createComment.EntityId, cancellationToken);
        intentionManager.ThrowIfForbidden(GameIntention.CreateComment, game);

        var comment = commentaryFactory.Create(createComment, identityProvider.Current.User.UserId);
        var createdComment = await repository.Create(comment, cancellationToken);
        await countersRepository.Increment(game.Id, UnreadEntryType.Message, cancellationToken);
        await invokedEventProducer.Send(EventType.NewGameComment, comment.CommentId);

        return createdComment;
    }
}