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
internal class CommentaryCreatingService : ICommentaryCreatingService
{
    private readonly IValidator<CreateComment> validator;
    private readonly IGameReadingService gameReadingService;
    private readonly IIntentionManager intentionManager;
    private readonly IIdentityProvider identityProvider;
    private readonly ICommentaryFactory commentaryFactory;
    private readonly ICommentaryCreatingRepository repository;
    private readonly IUnreadCountersRepository countersRepository;
    private readonly IInvokedEventProducer invokedEventProducer;

    /// <inheritdoc />
    public CommentaryCreatingService(
        IValidator<CreateComment> validator,
        IGameReadingService gameReadingService,
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ICommentaryFactory commentaryFactory,
        ICommentaryCreatingRepository repository,
        IUnreadCountersRepository countersRepository,
        IInvokedEventProducer invokedEventProducer)
    {
        this.validator = validator;
        this.gameReadingService = gameReadingService;
        this.intentionManager = intentionManager;
        this.commentaryFactory = commentaryFactory;
        this.repository = repository;
        this.countersRepository = countersRepository;
        this.invokedEventProducer = invokedEventProducer;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<Comment> Create(CreateComment createComment)
    {
        await validator.ValidateAndThrowAsync(createComment);

        var game = await gameReadingService.GetGame(createComment.EntityId);
        intentionManager.ThrowIfForbidden(GameIntention.CreateComment, game);

        var comment = commentaryFactory.Create(createComment, identityProvider.Current.User.UserId);
        var createdComment = await repository.Create(comment);
        await countersRepository.Increment(game.Id, UnreadEntryType.Message);
        await invokedEventProducer.Send(EventType.NewGameComment, comment.CommentId);

        return createdComment;
    }
}