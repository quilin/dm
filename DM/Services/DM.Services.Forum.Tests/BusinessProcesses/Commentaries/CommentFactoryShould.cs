using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.BusinessProcesses.Commentaries.Creating;
using DM.Services.Forum.Dto.Input;
using DM.Tests.Core;
using FluentAssertions;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Forum.Tests.BusinessProcesses.Commentaries
{
    public class CommentFactoryShould : UnitTestBase
    {
        private readonly ISetup<IGuidFactory, Guid> newIdSetup;
        private readonly ISetup<IDateTimeProvider, DateTime> currentMomentSetup;
        private readonly CommentaryFactory factory;

        public CommentFactoryShould()
        {
            var guidFactory = Mock<IGuidFactory>();
            newIdSetup = guidFactory.Setup(f => f.Create());

            var dateTimeProvider = Mock<IDateTimeProvider>();
            currentMomentSetup = dateTimeProvider.Setup(p => p.Now);

            factory = new CommentaryFactory(guidFactory.Object, dateTimeProvider.Object);
        }

        [Fact]
        public void CreateCommentaryModel()
        {
            var topicId = Guid.NewGuid();
            var createComment = new CreateComment
            {
                TopicId = topicId,
                Text = "text of commentary"
            };
            var userId = Guid.NewGuid();

            var commentId = Guid.NewGuid();
            newIdSetup.Returns(commentId);
            var rightNow = new DateTime(2017, 01, 01);
            currentMomentSetup.Returns(rightNow);

            var actual = factory.Create(createComment, userId);
            actual.Should().BeEquivalentTo(new ForumComment
            {
                ForumCommentId = commentId,
                ForumTopicId = topicId,
                CreateDate = rightNow,
                UserId = userId,
                LastUpdateDate = null,
                IsRemoved = false,
                Text = "text of commentary"
            });
        }
    }
}