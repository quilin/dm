using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Topics.Deleting;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.BusinessProcesses.Topics.Updating;
using DM.Services.Forum.Dto.Output;
using DM.Services.MessageQueuing.GeneralBus;
using DM.Tests.Core;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Forum.Tests.BusinessProcesses.Topics;

public class TopicDeletingServiceShould : UnitTestBase
{
    private readonly ISetup<ITopicReadingService, Task<Topic>> getTopicSetup;
    private readonly Mock<IIntentionManager> intentionManager;
    private readonly Mock<IUpdateBuilder<ForumTopic>> updateBuilder;
    private readonly Mock<ITopicUpdatingRepository> updatingRepository;
    private readonly ISetup<ITopicUpdatingRepository, Task<Topic>> updateSetup;
    private readonly Mock<IInvokedEventProducer> publisher;
    private readonly Mock<IUnreadCountersRepository> unreadCountersRepository;
    private readonly TopicDeletingService service;

    public TopicDeletingServiceShould()
    {
        var readingService = Mock<ITopicReadingService>();
        getTopicSetup = readingService.Setup(s => s.GetTopic(It.IsAny<Guid>()));

        intentionManager = Mock<IIntentionManager>();
        intentionManager
            .Setup(m => m.ThrowIfForbidden(It.IsAny<ForumIntention>(), It.IsAny<Topic>()));

        updateBuilder = Mock<IUpdateBuilder<ForumTopic>>();
        updateBuilder
            .Setup(b => b.Field(t => t.IsRemoved, It.IsAny<bool>()))
            .Returns(updateBuilder.Object);
        var updateBuilderFactory = Mock<IUpdateBuilderFactory>();
        updateBuilderFactory
            .Setup(f => f.Create<ForumTopic>(It.IsAny<Guid>()))
            .Returns(updateBuilder.Object);

        updatingRepository = Mock<ITopicUpdatingRepository>();
        updateSetup = updatingRepository.Setup(r => r.Update(It.IsAny<IUpdateBuilder<ForumTopic>>()));

        publisher = Mock<IInvokedEventProducer>();
        publisher
            .Setup(p => p.Send(It.IsAny<EventType>(), It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        unreadCountersRepository = Mock<IUnreadCountersRepository>();
        unreadCountersRepository
            .Setup(r => r.Delete(It.IsAny<Guid>(), It.IsAny<UnreadEntryType>()))
            .Returns(Task.CompletedTask);

        service = new TopicDeletingService(readingService.Object,
            intentionManager.Object, updateBuilderFactory.Object,
            updatingRepository.Object, publisher.Object,
            unreadCountersRepository.Object);
    }

    [Fact]
    public async Task AuthorizeDeletingAction()
    {
        var topicId = Guid.NewGuid();
        var forum = new Dto.Output.Forum();
        var topic = new Topic {Forum = forum};
        getTopicSetup.ReturnsAsync(topic);
        updateSetup.ReturnsAsync(new Topic());

        await service.DeleteTopic(topicId);

        intentionManager.Verify(m => m.ThrowIfForbidden(ForumIntention.AdministrateTopics, forum), Times.Once);
    }

    [Fact]
    public async Task OnlyUpdateRemovedField()
    {
        var topicId = Guid.NewGuid();
        var topic = new Topic();
        getTopicSetup.ReturnsAsync(topic);
        updateSetup.ReturnsAsync(new Topic());

        await service.DeleteTopic(topicId);

        updateBuilder.Verify(b => b.Field(t => t.IsRemoved, true), Times.Once);
        updateBuilder.VerifyNoOtherCalls();
        updatingRepository.Verify(r => r.Update(updateBuilder.Object), Times.Once);
        updatingRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task DeleteUnreadCounters()
    {
        var topicId = Guid.NewGuid();
        var topic = new Topic();
        getTopicSetup.ReturnsAsync(topic);
        updateSetup.ReturnsAsync(new Topic());

        await service.DeleteTopic(topicId);

        unreadCountersRepository.Verify(r => r.Delete(topicId, UnreadEntryType.Message), Times.Once);
        unreadCountersRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task PublishEvent()
    {
        var topicId = Guid.NewGuid();
        var topic = new Topic();
        getTopicSetup.ReturnsAsync(topic);
        updateSetup.ReturnsAsync(new Topic());

        await service.DeleteTopic(topicId);

        publisher.Verify(p => p.Send(EventType.DeletedForumTopic, topicId), Times.Once);
        publisher.VerifyNoOtherCalls();
    }
}