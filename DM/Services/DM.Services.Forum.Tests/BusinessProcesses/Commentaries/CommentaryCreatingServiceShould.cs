using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Implementation;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Commentaries;
using DM.Services.Forum.BusinessProcesses.Topics;
using DM.Services.Forum.Dto.Input;
using DM.Services.Forum.Dto.Output;
using DM.Services.MessageQueuing.Publish;
using DM.Tests.Core;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Forum.Tests.BusinessProcesses.Commentaries
{
    public class CommentaryCreatingServiceShould : UnitTestBase
    {
        private readonly ISetup<ITopicReadingService, Task<Topic>> topicReadingSetup;
        private readonly ISetup<IIdentity, AuthenticatedUser> currentUserSetup;
        private readonly ISetup<ICommentFactory, ForumComment> commentaryDalCreateSetup;
        private readonly ISetup<ICommentRepository, Task<Common.Dto.Comment>> commentaryCreateSetup;
        private readonly Mock<ICommentRepository> commentRepository;
        private readonly CommentCreatingService service;
        private readonly Mock<IValidator<CreateComment>> validator;
        private readonly Mock<ITopicReadingService> topicReadingService;
        private readonly Mock<ICommentFactory> commentFactory;
        private readonly Mock<IInvokedEventPublisher> invokedEventPublisher;

        public CommentaryCreatingServiceShould()
        {
            validator = Mock<IValidator<CreateComment>>();
            validator
                .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CreateComment>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            topicReadingService = Mock<ITopicReadingService>();
            topicReadingSetup = topicReadingService.Setup(s => s.GetTopic(It.IsAny<Guid>()));

            var intentionManager = Mock<IIntentionManager>();
            intentionManager
                .Setup(m => m.ThrowIfForbidden(It.IsAny<TopicIntention>(), It.IsAny<Topic>()))
                .Returns(Task.CompletedTask);

            var identityProvider = Mock<IIdentityProvider>();
            var identity = Mock<IIdentity>();
            identityProvider.Setup(p => p.Current).Returns(identity.Object);
            currentUserSetup = identity.Setup(i => i.User);

            commentFactory = Mock<ICommentFactory>();
            commentaryDalCreateSetup = commentFactory
                .Setup(f => f.Create(It.IsAny<CreateComment>(), It.IsAny<Guid>()));

            commentRepository = Mock<ICommentRepository>();
            commentaryCreateSetup = commentRepository.Setup(r => r.Create(It.IsAny<ForumComment>()));

            invokedEventPublisher = Mock<IInvokedEventPublisher>();
            invokedEventPublisher
                .Setup(p => p.Publish(It.IsAny<EventType>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            service = new CommentCreatingService(validator.Object, topicReadingService.Object,
                intentionManager.Object, identityProvider.Object, commentFactory.Object,
                commentRepository.Object, invokedEventPublisher.Object);
        }

        [Fact]
        public async Task CreateNewCommentary()
        {
            var topicId = Guid.NewGuid();
            var createComment = new CreateComment {TopicId = topicId};
            var topic = new Topic();
            topicReadingSetup.ReturnsAsync(topic);
            var userId = Guid.NewGuid();
            currentUserSetup.Returns(new AuthenticatedUser {UserId = userId});
            var commentId = Guid.NewGuid();
            var comment = new ForumComment {ForumCommentId = commentId};
            commentaryDalCreateSetup.Returns(comment);
            var expected = new Common.Dto.Comment();
            commentaryCreateSetup.ReturnsAsync(expected);

            var actual = await service.Create(createComment);
            actual.Should().Be(expected);

            validator.Verify(v => v.ValidateAsync(
                It.Is<ValidationContext<CreateComment>>(c => c.InstanceToValidate == createComment),
                It.IsAny<CancellationToken>()), Times.Once);

            topicReadingService.Verify(s => s.GetTopic(topicId), Times.Once);
            topicReadingService.VerifyNoOtherCalls();

            commentFactory.Verify(f => f.Create(createComment, userId));

            commentRepository.Verify(r => r.Create(comment), Times.Once);
            commentRepository.VerifyNoOtherCalls();

            invokedEventPublisher.Verify(p => p.Publish(EventType.NewForumComment, commentId), Times.Once);
            invokedEventPublisher.VerifyNoOtherCalls();
        }
    }
}