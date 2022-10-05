using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.BusinessProcesses.Topics.Creating;
using DM.Services.Forum.Dto.Input;
using DM.Services.Forum.Dto.Output;
using DM.Services.MessageQueuing.GeneralBus;
using DM.Tests.Core;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Forum.Tests.BusinessProcesses.Topics;

public class TopicCreatingServiceShould : UnitTestBase
{
    private readonly ISetup<IForumReadingService, Task<Dto.Output.Forum>> getForumSetup;
    private readonly Mock<IIntentionManager> intentionManager;
    private readonly ISetup<ITopicFactory, ForumTopic> createTopicSetup;
    private readonly Mock<ITopicCreatingRepository> creatingRepository;
    private readonly ISetup<ITopicCreatingRepository, Task<Topic>> saveTopicSetup;
    private readonly Mock<IUnreadCountersRepository> unreadCountersRepository;
    private readonly Mock<IInvokedEventProducer> publisher;
    private readonly TopicCreatingService service;

    public TopicCreatingServiceShould()
    {
        var validator = Mock<IValidator<CreateTopic>>();
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CreateTopic>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var forumReadingService = Mock<IForumReadingService>();
        getForumSetup = forumReadingService.Setup(s => s.GetForum(It.IsAny<string>(), true));

        intentionManager = Mock<IIntentionManager>();
        intentionManager
            .Setup(m => m.ThrowIfForbidden(It.IsAny<ForumIntention>(), It.IsAny<Dto.Output.Forum>()));

        var identityProvider = Mock<IIdentityProvider>();
        identityProvider
            .Setup(p => p.Current)
            .Returns(Identity.Guest);

        var topicFactory = Mock<ITopicFactory>();
        createTopicSetup = topicFactory.Setup(f => f.Create(
            It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CreateTopic>()));

        creatingRepository = Mock<ITopicCreatingRepository>();
        saveTopicSetup = creatingRepository.Setup(r => r.Create(It.IsAny<ForumTopic>()));

        unreadCountersRepository = Mock<IUnreadCountersRepository>();
        unreadCountersRepository
            .Setup(r => r.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<UnreadEntryType>()))
            .Returns(Task.CompletedTask);

        publisher = Mock<IInvokedEventProducer>();
        publisher
            .Setup(p => p.Send(It.IsAny<EventType>(), It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        service = new TopicCreatingService(validator.Object,
            forumReadingService.Object,
            intentionManager.Object,
            identityProvider.Object,
            topicFactory.Object,
            creatingRepository.Object,
            unreadCountersRepository.Object,
            publisher.Object);
    }

    [Fact]
    public async Task AuthorizeCreateTopicAction()
    {
        var createTopic = new CreateTopic {ForumTitle = "Forum X"};
        var forum = new Dto.Output.Forum {Id = Guid.NewGuid()};
        getForumSetup.ReturnsAsync(forum);
        var forumTopic = new ForumTopic();
        createTopicSetup.Returns(forumTopic);
        saveTopicSetup.ReturnsAsync(new Topic());

        await service.CreateTopic(createTopic);

        intentionManager.Verify(m => m.ThrowIfForbidden(ForumIntention.CreateTopic, forum));
    }

    [Fact]
    public async Task SaveTopic()
    {
        var createTopic = new CreateTopic {ForumTitle = "Forum Y"};
        var forum = new Dto.Output.Forum {Id = Guid.NewGuid()};
        getForumSetup.ReturnsAsync(forum);
        var forumTopic = new ForumTopic();
        createTopicSetup.Returns(forumTopic);
        var expected = new Topic();
        saveTopicSetup.ReturnsAsync(expected);

        var actual = await service.CreateTopic(createTopic);

        actual.Should().Be(expected);
        creatingRepository.Verify(r => r.Create(forumTopic), Times.Once);
        creatingRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateUnreadCommentsCounterForTopic()
    {
        var createTopic = new CreateTopic {ForumTitle = "Forum Y"};
        var forumId = Guid.NewGuid();
        var forum = new Dto.Output.Forum {Id = forumId};
        getForumSetup.ReturnsAsync(forum);
        var forumTopicId = Guid.NewGuid();
        var forumTopic = new ForumTopic {ForumTopicId = forumTopicId};
        createTopicSetup.Returns(forumTopic);
        saveTopicSetup.ReturnsAsync(new Topic {Id = forumTopicId});

        await service.CreateTopic(createTopic);

        unreadCountersRepository.Verify(r => r.Create(forumTopicId, forumId, UnreadEntryType.Message), Times.Once);
        unreadCountersRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task PublishMessage()
    {
        var createTopic = new CreateTopic {ForumTitle = "Forum Y"};
        var forum = new Dto.Output.Forum {Id = Guid.NewGuid()};
        getForumSetup.ReturnsAsync(forum);
        var forumTopicId = Guid.NewGuid();
        var forumTopic = new ForumTopic {ForumTopicId = forumTopicId};
        createTopicSetup.Returns(forumTopic);
        saveTopicSetup.ReturnsAsync(new Topic {Id = forumTopicId});

        await service.CreateTopic(createTopic);

        publisher.Verify(p => p.Send(EventType.NewForumTopic, forumTopicId), Times.Once);
        publisher.VerifyNoOtherCalls();
    }
}