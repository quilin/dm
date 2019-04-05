using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Implementation;
using DM.Services.Common.Repositories;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.Forum.Dto;
using DM.Services.Forum.Factories;
using DM.Services.Forum.Implementation;
using DM.Services.Forum.Repositories;
using DM.Services.MessageQueuing.Publish;
using DM.Tests.Core;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace DM.Services.Forum.Tests
{
    public class ForumServiceShould : UnitTestBase
    {
        private readonly ForumService service;
        private readonly Mock<IIdentity> identity;
        private readonly Mock<IAccessPolicyConverter> accessPolicyConverter;
        private readonly Mock<IForumRepository> forumRepository;
        private readonly Mock<IMemoryCache> memoryCache;
        private readonly Mock<ICacheEntry> forumCacheEntry;

        public ForumServiceShould()
        {
            var identityProvider = Mock<IIdentityProvider>();
            identity = Mock<IIdentity>();
            identityProvider.Setup(p => p.Current).Returns(identity.Object);
            accessPolicyConverter = Mock<IAccessPolicyConverter>();
            forumRepository = Mock<IForumRepository>();
            memoryCache = Mock<IMemoryCache>();
            memoryCache
                .Setup(c => c.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                .Returns(false);
            forumCacheEntry = Mock<ICacheEntry>();
            forumCacheEntry.Setup(e => e.Dispose());
            memoryCache
                .Setup(c => c.CreateEntry(It.IsAny<object>()))
                .Returns(forumCacheEntry.Object);
            service = new ForumService(Mock<IValidator<CreateTopic>>().Object,
                Mock<IValidator<UpdateTopic>>().Object, identityProvider.Object,
                accessPolicyConverter.Object, Mock<IIntentionManager>().Object,
                Mock<ITopicFactory>().Object, Mock<ILikeFactory>().Object,
                Mock<IUnreadCountersRepository>().Object, forumRepository.Object,
                Mock<ITopicRepository>().Object, Mock<IModeratorRepository>().Object,
                Mock<ILikeRepository>().Object, Mock<ICommentRepository>().Object,
                memoryCache.Object, Mock<IInvokedEventPublisher>().Object);
        }

        [Fact]
        public async Task ThrowHttpNotFoundExceptionWhenForumNotFound()
        {
            identity.Setup(i => i.User).Returns(new AuthenticatedUser {Role = UserRole.NannyModerator});
            accessPolicyConverter
                .Setup(c => c.Convert(UserRole.NannyModerator))
                .Returns(ForumAccessPolicy.ForumModerator);
            var forums = new[] {new Dto.Forum {Title = "Such forum!"}};
            forumRepository
                .Setup(r => r.SelectFora(It.IsAny<ForumAccessPolicy?>()))
                .ReturnsAsync(forums);
            forumCacheEntry.SetupSet(e => e.Value = forums);

            service.Invoking(s => service.GetForum("No such forum").Wait())
                .Should().Throw<HttpException>()
                .And.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}