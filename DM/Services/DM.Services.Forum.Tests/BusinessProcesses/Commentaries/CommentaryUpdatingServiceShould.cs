using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Commentaries.Reading;
using DM.Services.Forum.BusinessProcesses.Commentaries.Updating;
using DM.Services.Forum.Dto.Input;
using DM.Services.MessageQueuing.Publish;
using DM.Tests.Core;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using Xunit;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Forum.Tests.BusinessProcesses.Commentaries
{
    public class CommentaryUpdatingServiceShould : UnitTestBase
    {
        private readonly ISetup<ICommentaryReadingService, Task<Dto.Output.Comment>> getCommentSetup;
        private readonly Mock<IIntentionManager> intentionManager;
        private readonly ISetup<IDateTimeProvider, DateTimeOffset> currentMomentSetup;
        private readonly Mock<IUpdateBuilder<Comment>> commentUpdateBuilder;
        private readonly Mock<ICommentaryUpdatingRepository> commentRepository;
        private readonly ISetup<ICommentaryUpdatingRepository, Task<Dto.Output.Comment>> updateCommentSetup;
        private readonly Mock<IInvokedEventPublisher> eventPublisher;
        private readonly CommentaryUpdatingService service;

        public CommentaryUpdatingServiceShould()
        {
            var validator = Mock<IValidator<UpdateComment>>();
            validator
                .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<UpdateComment>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var readingService = Mock<ICommentaryReadingService>();
            getCommentSetup = readingService.Setup(s => s.Get(It.IsAny<Guid>()));

            intentionManager = Mock<IIntentionManager>();
            intentionManager
                .Setup(m => m.ThrowIfForbidden(It.IsAny<CommentIntention>(), It.IsAny<Dto.Output.Comment>()))
                .Returns(Task.CompletedTask);

            var dateTimeProvider = Mock<IDateTimeProvider>();
            currentMomentSetup = dateTimeProvider.Setup(p => p.Now);

            var updateBuilderFactory = Mock<IUpdateBuilderFactory>();
            commentUpdateBuilder = MockUpdateBuilder<Comment>();
            updateBuilderFactory
                .Setup(f => f.Create<Comment>(It.IsAny<Guid>()))
                .Returns(commentUpdateBuilder.Object);

            commentRepository = Mock<ICommentaryUpdatingRepository>();
            updateCommentSetup = commentRepository.Setup(r => r.Update(It.IsAny<IUpdateBuilder<Comment>>()));

            eventPublisher = Mock<IInvokedEventPublisher>();
            eventPublisher
                .Setup(p => p.Publish(It.IsAny<EventType>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            service = new CommentaryUpdatingService(
                validator.Object,
                readingService.Object,
                intentionManager.Object,
                dateTimeProvider.Object,
                updateBuilderFactory.Object,
                commentRepository.Object,
                eventPublisher.Object);
        }

        [Fact]
        public async Task AuthorizeUpdateAction()
        {
            var commentId = Guid.NewGuid();
            var comment = new Dto.Output.Comment();
            getCommentSetup.ReturnsAsync(comment);
            var updatedComment = new Dto.Output.Comment();
            updateCommentSetup.ReturnsAsync(updatedComment);

            await service.Update(new UpdateComment {CommentId = commentId, Text = string.Empty});

            intentionManager.Verify(m => m.ThrowIfForbidden(CommentIntention.Edit, comment), Times.Once);
        }

        [Fact]
        public async Task SaveWithUpdatedTextAndModifiedDate()
        {
            var commentId = Guid.NewGuid();
            var comment = new Dto.Output.Comment();
            getCommentSetup.ReturnsAsync(comment);
            var expected = new Dto.Output.Comment();
            updateCommentSetup.ReturnsAsync(expected);
            var rightNow = new DateTimeOffset(2019, 01, 12, 10, 00, 00, TimeSpan.Zero);
            currentMomentSetup.Returns(rightNow);

            var actual = await service.Update(new UpdateComment {CommentId = commentId, Text = "some text boi"});

            actual.Should().Be(expected);
            commentRepository.Verify(r => r.Update(commentUpdateBuilder.Object), Times.Once);
            commentUpdateBuilder.Verify(b => b.Field(c => c.Text, "some text boi"));
            commentUpdateBuilder.Verify(b => b.Field(c => c.LastUpdateDate, rightNow));
        }

        [Fact]
        public async Task PublishEvent()
        {
            var commentId = Guid.NewGuid();
            var comment = new Dto.Output.Comment();
            getCommentSetup.ReturnsAsync(comment);
            var updatedComment = new Dto.Output.Comment();
            updateCommentSetup.ReturnsAsync(updatedComment);

            await service.Update(new UpdateComment {CommentId = commentId, Text = string.Empty});

            eventPublisher.Verify(p => p.Publish(EventType.ChangedForumComment, commentId), Times.Once);
        }
    }
}