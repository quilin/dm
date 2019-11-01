using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.Commentaries;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;
using Comment = DM.Services.Common.Dto.Comment;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Creating
{
    /// <inheritdoc />
    public class CommentaryCreatingService : ICommentaryCreatingService
    {
        private readonly IValidator<CreateComment> validator;
        private readonly IGameReadingService gameReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly IIdentity identity;
        private readonly ICommentaryFactory commentaryFactory;
        private readonly ICommentaryCreatingRepository repository;
        private readonly IUnreadCountersRepository countersRepository;
        private readonly IInvokedEventPublisher invokedEventPublisher;

        /// <inheritdoc />
        public CommentaryCreatingService(
            IValidator<CreateComment> validator,
            IGameReadingService gameReadingService,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            ICommentaryFactory commentaryFactory,
            ICommentaryCreatingRepository repository,
            IUnreadCountersRepository countersRepository,
            IInvokedEventPublisher invokedEventPublisher)
        {
            this.validator = validator;
            this.gameReadingService = gameReadingService;
            this.intentionManager = intentionManager;
            this.commentaryFactory = commentaryFactory;
            this.repository = repository;
            this.countersRepository = countersRepository;
            this.invokedEventPublisher = invokedEventPublisher;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public async Task<Comment> Create(CreateComment createComment)
        {
            await validator.ValidateAndThrowAsync(createComment);

            var game = await gameReadingService.GetGame(createComment.EntityId);
            await intentionManager.ThrowIfForbidden(GameIntention.CreateComment, game);

            var comment = commentaryFactory.Create(createComment, identity.User.UserId);
            var createdComment = await repository.Create(comment);
            await countersRepository.Increment(game.Id, UnreadEntryType.Message);
            await invokedEventPublisher.Publish(EventType.NewGameComment, comment.CommentId);

            return createdComment;
        }
    }
}