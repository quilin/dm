using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.Likes;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Commentaries.Reading;
using DM.Services.Forum.BusinessProcesses.Likes;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.Dto.Output;
using DM.Services.Forum.Tests.Dsl;
using DM.Services.MessageQueuing.GeneralBus;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Forum.Tests.BusinessProcesses.Likes;

public class LikeServiceForTopicsShould
{
    private readonly LikeService service;
    private readonly ISetup<ITopicReadingService, Task<Topic>> topicReading;
    private readonly ISetup<IIdentity, AuthenticatedUser> currentUser;
    private readonly Mock<ILikeFactory> factory;
    private readonly Mock<ILikeRepository> likeRepository;
    private readonly Mock<IInvokedEventProducer> publisher;

    public LikeServiceForTopicsShould()
    {
        var topicReadingService = new Mock<ITopicReadingService>();
        topicReading = topicReadingService.Setup(s => s.GetTopic(It.IsAny<Guid>()));

        var intentionManager = new Mock<IIntentionManager>();
        intentionManager
            .Setup(m => m.ThrowIfForbidden(TopicIntention.Like, It.IsAny<Topic>()));
        var identityProvider = new Mock<IIdentityProvider>();
        var identity = new Mock<IIdentity>();
        currentUser = identity.Setup(i => i.User);
        identityProvider.Setup(p => p.Current).Returns(identity.Object);

        factory = new Mock<ILikeFactory>();
        likeRepository = new Mock<ILikeRepository>();
        publisher = new Mock<IInvokedEventProducer>();
        service = new LikeService(topicReadingService.Object, new Mock<ICommentaryReadingService>().Object,
            intentionManager.Object, identityProvider.Object, factory.Object,
            likeRepository.Object, publisher.Object);
    }

    [Fact]
    public void ThrowConflictExceptionWhenUserTriesToLikeTopicTwice()
    {
        var topicId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        topicReading.ReturnsAsync(new Topic
        {
            Likes = new List<GeneralUser>
            {
                new() {UserId = userId},
                new() {UserId = Guid.NewGuid()}
            }
        });
        currentUser.Returns(Create.User(userId).Please);

        service.Invoking(s => s.LikeTopic(topicId).Wait())
            .Should().Throw<HttpException>()
            .And.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task SaveInRepositoryAndPublishMessageAndReturnUserWhenLikes()
    {
        var topicId = Guid.NewGuid();
        topicReading.ReturnsAsync(new Topic
        {
            Id = topicId,
            Likes = new List<GeneralUser>
            {
                new() {UserId = Guid.NewGuid()},
                new() {UserId = Guid.NewGuid()}
            }
        });
        var user = Create.User().Please();
        currentUser.Returns(user);

        var likeId = Guid.NewGuid();
        var like = new Like {LikeId = likeId};
        factory
            .Setup(f => f.Create(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Returns(like);
        likeRepository
            .Setup(r => r.Add(It.IsAny<Like>()))
            .Returns(Task.CompletedTask);
        publisher
            .Setup(p => p.Send(It.IsAny<EventType>(), It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        var actual = await service.LikeTopic(topicId);
        actual.Should().Be(user);

        likeRepository.Verify(r => r.Add(like), Times.Once);
        likeRepository.VerifyNoOtherCalls();

        publisher.Verify(p => p.Send(EventType.LikedTopic, likeId), Times.Once);
        publisher.VerifyNoOtherCalls();
    }

    [Fact]
    public void ThrowConflictExceptionWhenUserTriesToDislikeHeNeverLiked()
    {
        var topicId = Guid.NewGuid();
        topicReading.ReturnsAsync(new Topic
        {
            Likes = new List<GeneralUser>
            {
                new() {UserId = Guid.NewGuid()}
            }
        });
        currentUser.Returns(Create.User().Please);

        service.Invoking(s => s.DislikeTopic(topicId).Wait())
            .Should().Throw<HttpException>()
            .And.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task SaveInRepositoryWhenDislikes()
    {
        var topicId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        topicReading.ReturnsAsync(new Topic
        {
            Id = topicId,
            Likes = new List<GeneralUser>
            {
                new() {UserId = userId},
                new() {UserId = Guid.NewGuid()}
            }
        });
        var user = Create.User(userId).Please();
        currentUser.Returns(user);

        likeRepository
            .Setup(r => r.Delete(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        await service.DislikeTopic(topicId);

        likeRepository.Verify(r => r.Delete(topicId, userId), Times.Once);
        likeRepository.VerifyNoOtherCalls();
    }
}