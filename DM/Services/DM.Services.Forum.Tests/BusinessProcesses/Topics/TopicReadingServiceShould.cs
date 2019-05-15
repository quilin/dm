using System;
using System.Net;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Repositories;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.Forum.BusinessProcesses.Common;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.Dto.Output;
using DM.Services.Forum.Tests.Dsl;
using DM.Tests.Core;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Forum.Tests.BusinessProcesses.Topics
{
    public class TopicReadingServiceShould : UnitTestBase
    {
        private readonly TopicReadingService service;
        private readonly ISetup<IIdentity, AuthenticatedUser> currentUserSetup;
        private readonly Mock<ITopicReadingRepository> topicRepository;
        private readonly Mock<IAccessPolicyConverter> accessPolicyConverter;

        public TopicReadingServiceShould()
        {
            var identityProvider = Mock<IIdentityProvider>();
            var identity = Mock<IIdentity>();
            identityProvider.Setup(p => p.Current).Returns(identity.Object);
            currentUserSetup = identity.Setup(i => i.User);
            topicRepository = Mock<ITopicReadingRepository>();
            accessPolicyConverter = Mock<IAccessPolicyConverter>();
            service = new TopicReadingService(identityProvider.Object,
                Mock<IForumReadingService>().Object, accessPolicyConverter.Object,
                topicRepository.Object, Mock<IUnreadCountersRepository>().Object);
        }

        [Fact]
        public void ThrowHttpExceptionWhenAvailableTopicNotFound()
        {
            var topicId = Guid.NewGuid();
            currentUserSetup.Returns(Create.User().Please);
            accessPolicyConverter
                .Setup(c => c.Convert(It.IsAny<UserRole>()))
                .Returns(ForumAccessPolicy.SeniorModerator);

            topicRepository
                .Setup(r => r.Get(It.IsAny<Guid>(), It.IsAny<ForumAccessPolicy>()))
                .ReturnsAsync((Topic) null);

            service.Invoking(s => s.GetTopic(topicId).Wait())
                .Should().Throw<HttpException>()
                .And.StatusCode.Should().Be(HttpStatusCode.NotFound);

            topicRepository.Verify(r => r.Get(topicId, ForumAccessPolicy.SeniorModerator), Times.Once);
            topicRepository.VerifyNoOtherCalls();
        }
    }
}