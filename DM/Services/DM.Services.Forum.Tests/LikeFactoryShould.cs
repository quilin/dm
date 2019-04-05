using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.Factories;
using DM.Tests.Core;
using FluentAssertions;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Forum.Tests
{
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
}