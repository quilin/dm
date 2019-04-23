using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Implementation;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Commentaries;
using DM.Services.Forum.BusinessProcesses.Likes;
using DM.Services.Forum.BusinessProcesses.Topics;
using DM.Services.Forum.Dto.Output;
using DM.Services.MessageQueuing.Publish;
using DM.Tests.Core;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Forum.Tests.BusinessProcesses.Likes
{
    public class LikeServiceShould : UnitTestBase
    {
        private readonly LikeService service;
        private readonly ISetup<ITopicReadingService, Task<Topic>> topicReading;
        private readonly ISetup<IIdentity, AuthenticatedUser> currentUser;
        private readonly Mock<ILikeFactory> factory;
        private readonly Mock<ILikeRepository> likeRepository;
        private readonly Mock<IInvokedEventPublisher> publisher;

        public LikeServiceShould()
        {
            var topicReadingService = Mock<ITopicReadingService>();
            topicReading = topicReadingService.Setup(s => s.GetTopic(It.IsAny<Guid>()));

            var intentionManager = Mock<IIntentionManager>();
            intentionManager
                .Setup(m => m.ThrowIfForbidden(TopicIntention.Like, It.IsAny<Topic>()))
                .Returns(Task.CompletedTask);
            var identityProvider = Mock<IIdentityProvider>();
            var identity = Mock<IIdentity>();
            currentUser = identity.Setup(i => i.User);
            identityProvider.Setup(p => p.Current).Returns(identity.Object);

            factory = Mock<ILikeFactory>();
            likeRepository = Mock<ILikeRepository>();
            publisher = Mock<IInvokedEventPublisher>();
            service = new LikeService(topicReadingService.Object, Mock<ICommentaryReadingService>().Object,
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
                    new GeneralUser {UserId = userId},
                    new GeneralUser {UserId = Guid.NewGuid()}
                }
            });
            currentUser.Returns(new AuthenticatedUser {UserId = userId});

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
                    new GeneralUser {UserId = Guid.NewGuid()},
                    new GeneralUser {UserId = Guid.NewGuid()}
                }
            });
            var user = new AuthenticatedUser {UserId = Guid.NewGuid()};
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
                .Setup(p => p.Publish(It.IsAny<EventType>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            var actual = await service.LikeTopic(topicId);
            actual.Should().Be(user);

            likeRepository.Verify(r => r.Add(like), Times.Once);
            likeRepository.VerifyNoOtherCalls();

            publisher.Verify(p => p.Publish(EventType.LikedTopic, likeId), Times.Once);
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
                    new GeneralUser {UserId = Guid.NewGuid()}
                }
            });
            currentUser.Returns(new AuthenticatedUser {UserId = Guid.NewGuid()});

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
                    new GeneralUser {UserId = userId},
                    new GeneralUser {UserId = Guid.NewGuid()}
                }
            });
            var user = new AuthenticatedUser {UserId = userId};
            currentUser.Returns(user);

            likeRepository
                .Setup(r => r.Delete(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            await service.DislikeTopic(topicId);

            likeRepository.Verify(r => r.Delete(topicId, userId), Times.Once);
            likeRepository.VerifyNoOtherCalls();
        }
    }
}