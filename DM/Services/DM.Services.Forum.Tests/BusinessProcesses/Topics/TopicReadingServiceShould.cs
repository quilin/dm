using System;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Repositories;
using DM.Services.Core.Exceptions;
using DM.Services.Forum.BusinessProcesses.Common;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.BusinessProcesses.Topics;
using DM.Tests.Core;
using FluentAssertions;
using Xunit;

namespace DM.Services.Forum.Tests.BusinessProcesses.Topics
{
    public class TopicReadingServiceShould : UnitTestBase
    {
        private readonly TopicReadingService service;

        public TopicReadingServiceShould()
        {
            var identityProvider = Mock<IIdentityProvider>();
            var identity = Mock<IIdentity>();
            identityProvider.Setup(p => p.Current).Returns(identity.Object);
            service = new TopicReadingService(identityProvider.Object,
                Mock<IForumReadingService>().Object, Mock<IAccessPolicyConverter>().Object,
                Mock<ITopicRepository>().Object, Mock<IUnreadCountersRepository>().Object);
        }

        [Fact]
        public async Task ThrowHttpExceptionWhenAvailableTopicNotFound()
        {
            var topicId = Guid.NewGuid();
            service.Invoking(s => s.GetTopic(topicId).Wait())
                .Should().Throw<HttpException>()
                .And.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}