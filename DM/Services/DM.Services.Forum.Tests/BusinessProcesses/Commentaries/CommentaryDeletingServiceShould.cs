using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Commentaries.Deleting;
using DM.Services.Forum.Dto.Internal;
using DM.Services.MessageQueuing.GeneralBus;
using DM.Tests.Core;
using Moq;
using Moq.Language.Flow;
using Xunit;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Forum.Tests.BusinessProcesses.Commentaries;

public class CommentaryDeletingServiceShould : UnitTestBase
{
    private readonly Mock<IIntentionManager> intentionManager;
    private readonly Mock<IUpdateBuilder<ForumTopic>> topicUpdateBuilder;
    private readonly Mock<ICommentaryDeletingRepository> commentaryRepository;
    private readonly Mock<IUnreadCountersRepository> unreadCountersRepository;
    private readonly Mock<IInvokedEventProducer> eventPublisher;
    private readonly CommentaryDeletingService service;
    private readonly ISetup<ICommentaryDeletingRepository, Task<CommentToDelete>> getCommentSetup;
    private readonly Mock<IUpdateBuilder<Comment>> commentUpdateBuilder;

    public CommentaryDeletingServiceShould()
    {
        intentionManager = Mock<IIntentionManager>();
        intentionManager
            .Setup(m => m.ThrowIfForbidden(It.IsAny<CommentIntention>(), It.IsAny<Common.Dto.Comment>()));

        var updateBuilderFactory = Mock<IUpdateBuilderFactory>();
        topicUpdateBuilder = Mock<IUpdateBuilder<ForumTopic>>();
        topicUpdateBuilder
            .Setup(b => b.Field(t => t.LastCommentId, It.IsAny<Guid?>()))
            .Returns(topicUpdateBuilder.Object);
        commentUpdateBuilder = Mock<IUpdateBuilder<Comment>>();
        commentUpdateBuilder
            .Setup(b => b.Field(c => c.IsRemoved, It.IsAny<bool>()))
            .Returns(commentUpdateBuilder.Object);
        updateBuilderFactory
            .Setup(f => f.Create<ForumTopic>(It.IsAny<Guid>()))
            .Returns(topicUpdateBuilder.Object);
        updateBuilderFactory
            .Setup(f => f.Create<Comment>(It.IsAny<Guid>()))
            .Returns(commentUpdateBuilder.Object);

        commentaryRepository = Mock<ICommentaryDeletingRepository>();
        getCommentSetup = commentaryRepository.Setup(r => r.GetForDelete(It.IsAny<Guid>()));

        unreadCountersRepository = Mock<IUnreadCountersRepository>();
        eventPublisher = Mock<IInvokedEventProducer>();

        service = new CommentaryDeletingService(
            intentionManager.Object, updateBuilderFactory.Object,
            commentaryRepository.Object, unreadCountersRepository.Object, eventPublisher.Object);
    }

    [Fact]
    public async Task AuthorizeDeleteAction()
    {
        var commentId = Guid.NewGuid();
        var comment = new CommentToDelete();
        getCommentSetup.ReturnsAsync(comment);

        await service.Delete(commentId);

        intentionManager.Verify(m => m.ThrowIfForbidden(CommentIntention.Delete, (Common.Dto.Comment) comment));
    }

    [Fact]
    public async Task SaveWithRemovedFlag()
    {
        var commentId = Guid.NewGuid();
        var comment = new CommentToDelete();
        getCommentSetup.ReturnsAsync(comment);

        await service.Delete(commentId);

        commentaryRepository.Verify(r =>
            r.Delete(commentUpdateBuilder.Object, topicUpdateBuilder.Object), Times.Once);
        commentUpdateBuilder.Verify(b => b.Field(c => c.IsRemoved, true));
    }

    [Fact]
    public async Task SaveWithUpdateTopicLastComment_WhenIsLastCommentOfTopic()
    {
        var commentId = Guid.NewGuid();
        var topicId = Guid.NewGuid();
        var comment = new CommentToDelete {IsLastCommentOfTopic = true, EntityId = topicId};
        getCommentSetup.ReturnsAsync(comment);

        var secondLastCommentId = Guid.NewGuid();
        commentaryRepository
            .Setup(r => r.GetSecondLastCommentId(topicId))
            .ReturnsAsync(secondLastCommentId);

        await service.Delete(commentId);

        commentaryRepository.Verify(r => r.GetSecondLastCommentId(topicId), Times.Once);
        commentaryRepository.Verify(r =>
            r.Delete(commentUpdateBuilder.Object, topicUpdateBuilder.Object), Times.Once);
        topicUpdateBuilder.Verify(b => b.Field(t => t.LastCommentId, secondLastCommentId));
    }

    [Fact]
    public async Task DecrementUnreadCounter()
    {
        var commentId = Guid.NewGuid();
        var topicId = Guid.NewGuid();
        var comment = new CommentToDelete
        {
            EntityId = topicId,
            CreateDate = new DateTimeOffset(2019, 01, 14, 10, 13, 11, TimeSpan.Zero)
        };
        getCommentSetup.ReturnsAsync(comment);

        await service.Delete(commentId);

        unreadCountersRepository.Verify(
            r => r.Decrement(topicId, UnreadEntryType.Message,
                new DateTimeOffset(2019, 01, 14, 10, 13, 11, TimeSpan.Zero)), Times.Once);
        unreadCountersRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SendEvent()
    {
        var commentId = Guid.NewGuid();
        var comment = new CommentToDelete();
        getCommentSetup.ReturnsAsync(comment);

        await service.Delete(commentId);

        eventPublisher.Verify(p => p.Send(EventType.DeletedForumComment, commentId));
    }
}