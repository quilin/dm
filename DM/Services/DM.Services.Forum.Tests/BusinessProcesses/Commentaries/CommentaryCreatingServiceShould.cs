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
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Commentaries.Creating;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.Dto.Input;
using DM.Services.Forum.Dto.Output;
using DM.Services.Forum.Tests.Dsl;
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
        private readonly ISetup<ICommentaryFactory, ForumComment> commentaryDalCreateSetup;
        private readonly ISetup<ICommentaryCreatingRepository, Task<Comment>> commentaryCreateSetup;
        private readonly Mock<ICommentaryCreatingRepository> commentRepository;
        private readonly Mock<IUnreadCountersRepository> countersRepository;
        private readonly CommentaryCreatingService service;
        private readonly Mock<IValidator<CreateComment>> validator;
        private readonly Mock<ITopicReadingService> topicReadingService;
        private readonly Mock<ICommentaryFactory> commentFactory;
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

            commentFactory = Mock<ICommentaryFactory>();
            commentaryDalCreateSetup = commentFactory
                .Setup(f => f.Create(It.IsAny<CreateComment>(), It.IsAny<Guid>()));

            commentRepository = Mock<ICommentaryCreatingRepository>();
            commentaryCreateSetup = commentRepository.Setup(r =>
                r.Create(It.IsAny<ForumComment>(), It.IsAny<UpdateBuilder<ForumTopic>>()));

            invokedEventPublisher = Mock<IInvokedEventPublisher>();
            invokedEventPublisher
                .Setup(p => p.Publish(It.IsAny<EventType>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            countersRepository = Mock<IUnreadCountersRepository>();
            countersRepository
                .Setup(r => r.Increment(It.IsAny<Guid>(), It.IsAny<UnreadEntryType>()))
                .Returns(Task.CompletedTask);

            service = new CommentaryCreatingService(validator.Object, topicReadingService.Object,
                intentionManager.Object, identityProvider.Object, commentFactory.Object,
                commentRepository.Object, countersRepository.Object, invokedEventPublisher.Object);
        }

        [Fact]
        public async Task CreateNewCommentary()
        {
            var topicId = Guid.NewGuid();
            var createComment = new CreateComment {TopicId = topicId};
            var topic = new Topic {Id = topicId};
            topicReadingSetup.ReturnsAsync(topic);
            var userId = Guid.NewGuid();
            currentUserSetup.Returns(Create.User(userId).Please);
            var commentId = Guid.NewGuid();
            var comment = new ForumComment {ForumCommentId = commentId};
            commentaryDalCreateSetup.Returns(comment);
            var expected = new Comment();
            commentaryCreateSetup.ReturnsAsync(expected);

            var actual = await service.Create(createComment);
            actual.Should().Be(expected);

            topicReadingService.Verify(s => s.GetTopic(topicId), Times.Once);
            topicReadingService.VerifyNoOtherCalls();

            commentRepository.Verify(r => r.Create(comment, It.IsAny<UpdateBuilder<ForumTopic>>()), Times.Once);
            commentRepository.VerifyNoOtherCalls();

            countersRepository.Verify(r => r.Increment(topicId, UnreadEntryType.Message));
            countersRepository.VerifyNoOtherCalls();

            invokedEventPublisher.Verify(p => p.Publish(EventType.NewForumComment, commentId), Times.Once);
            invokedEventPublisher.VerifyNoOtherCalls();
        }
    }
}