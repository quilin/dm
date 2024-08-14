using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.Commentaries;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;
using Comment = DM.Services.Common.Dto.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Creating;

/// <inheritdoc />
internal class CommentaryCreatingService : ICommentaryCreatingService
{
    private readonly IValidator<CreateComment> validator;
    private readonly ITopicReadingService topicReadingService;
    private readonly IIntentionManager intentionManager;
    private readonly IIdentityProvider identityProvider;
    private readonly ICommentaryFactory commentaryFactory;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly ICommentaryCreatingRepository repository;
    private readonly IUnreadCountersRepository countersRepository;
    private readonly IInvokedEventProducer invokedEventProducer;

    /// <inheritdoc />
    public CommentaryCreatingService(
        IValidator<CreateComment> validator,
        ITopicReadingService topicReadingService,
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ICommentaryFactory commentaryFactory,
        IUpdateBuilderFactory updateBuilderFactory,
        ICommentaryCreatingRepository repository,
        IUnreadCountersRepository countersRepository,
        IInvokedEventProducer invokedEventProducer)
    {
        this.validator = validator;
        this.topicReadingService = topicReadingService;
        this.intentionManager = intentionManager;
        this.commentaryFactory = commentaryFactory;
        this.updateBuilderFactory = updateBuilderFactory;
        this.repository = repository;
        this.countersRepository = countersRepository;
        this.invokedEventProducer = invokedEventProducer;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<Comment> Create(CreateComment createComment)
    {
        await validator.ValidateAndThrowAsync(createComment);

        var topic = await topicReadingService.GetTopic(createComment.EntityId);
        intentionManager.ThrowIfForbidden(TopicIntention.CreateComment, topic);

        var comment = commentaryFactory.Create(createComment, identityProvider.Current.User.UserId);
        var topicUpdate = updateBuilderFactory.Create<ForumTopic>(topic.Id)
            .Field(t => t.LastCommentId, comment.CommentId);
        var createdComment = await repository.Create(comment, topicUpdate);
        await countersRepository.Increment(topic.Id, UnreadEntryType.Message);
        await invokedEventProducer.Send(EventType.NewForumComment, comment.CommentId);

        return createdComment;
    }
}