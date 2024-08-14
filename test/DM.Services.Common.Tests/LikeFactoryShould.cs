using System;
using DM.Services.Common.BusinessProcesses.Likes;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Tests.Core;
using FluentAssertions;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Common.Tests;

public class LikeFactoryShould : UnitTestBase
{
    private readonly ISetup<IGuidFactory, Guid> idSetup;
    private readonly LikeFactory factory;

    public LikeFactoryShould()
    {
        var guidFactory = Mock<IGuidFactory>();
        idSetup = guidFactory.Setup(f => f.Create());
        factory = new LikeFactory(guidFactory.Object);
    }

    [Fact]
    public void CreateLike()
    {
        var entityId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var likeId = Guid.NewGuid();
        idSetup.Returns(likeId);

        var actual = factory.Create(entityId, userId);

        actual.Should().BeEquivalentTo(new Like
        {
            LikeId = likeId,
            UserId = userId,
            EntityId = entityId
        });
    }
}