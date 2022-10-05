using System;
using DM.Services.Common.BusinessProcesses.Commentaries;
using DM.Services.Common.Dto;
using DM.Services.Core.Implementation;
using DM.Tests.Core;
using FluentAssertions;
using Moq.Language.Flow;
using Xunit;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Common.Tests;

public class CommentFactoryShould : UnitTestBase
{
    private readonly ISetup<IGuidFactory, Guid> newIdSetup;
    private readonly ISetup<IDateTimeProvider, DateTimeOffset> currentMomentSetup;
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
            EntityId = topicId,
            Text = "text of commentary"
        };
        var userId = Guid.NewGuid();

        var commentId = Guid.NewGuid();
        newIdSetup.Returns(commentId);
        var rightNow = new DateTime(2017, 01, 01);
        currentMomentSetup.Returns(rightNow);

        var actual = factory.Create(createComment, userId);
        actual.Should().BeEquivalentTo(new Comment
        {
            CommentId = commentId,
            EntityId = topicId,
            CreateDate = rightNow,
            UserId = userId,
            LastUpdateDate = null,
            IsRemoved = false,
            Text = "text of commentary"
        });
    }
}