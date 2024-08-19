using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
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

namespace DM.Services.Forum.Tests.BusinessProcesses.Topics;

public class TopicReadingServiceShould : UnitTestBase
{
    private readonly TopicReadingService service;
    private readonly ISetup<IIdentity, AuthenticatedUser> currentUserSetup;
    private readonly Mock<ITopicReadingRepository> topicRepository;
    private readonly Mock<IAccessPolicyConverter> accessPolicyConverter;
    private readonly Mock<IUnreadCountersRepository> unreadCountersRepository;

    public TopicReadingServiceShould()
    {
        var identityProvider = Mock<IIdentityProvider>();
        var identity = Mock<IIdentity>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);
        currentUserSetup = identity.Setup(i => i.User);
        topicRepository = Mock<ITopicReadingRepository>();
        accessPolicyConverter = Mock<IAccessPolicyConverter>();
        unreadCountersRepository = Mock<IUnreadCountersRepository>();
        service = new TopicReadingService(identityProvider.Object,
            Mock<IForumReadingService>().Object, accessPolicyConverter.Object,
            topicRepository.Object, unreadCountersRepository.Object);
    }

    [Fact]
    public async Task ThrowHttpExceptionWhenAvailableTopicNotFound()
    {
        var topicId = Guid.NewGuid();
        currentUserSetup.Returns(Create.User().Please);
        accessPolicyConverter
            .Setup(c => c.Convert(It.IsAny<UserRole>()))
            .Returns(ForumAccessPolicy.SeniorModerator);

        topicRepository
            .Setup(r => r.Get(It.IsAny<Guid>(), It.IsAny<ForumAccessPolicy>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Topic) null);

        (await service.Invoking(s => s.GetTopic(topicId, CancellationToken.None))
            .Should().ThrowAsync<HttpException>())
            .And.StatusCode.Should().Be(HttpStatusCode.Gone);

        topicRepository.Verify(r => r.Get(topicId, ForumAccessPolicy.SeniorModerator, It.IsAny<CancellationToken>()), Times.Once);
        topicRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ReturnFoundTopic()
    {
        var topicId = Guid.NewGuid();
        var expected = new Topic();
        topicRepository
            .Setup(r => r.Get(It.IsAny<Guid>(), It.IsAny<ForumAccessPolicy>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        accessPolicyConverter
            .Setup(c => c.Convert(It.IsAny<UserRole>()))
            .Returns(ForumAccessPolicy.Player);
        currentUserSetup.Returns(Create.User().Please);

        var actual = await service.GetTopic(topicId, CancellationToken.None);

        actual.Should().Be(expected);
        topicRepository.Verify(r => r.Get(topicId, ForumAccessPolicy.Player, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FillUnreadCountersWhenUserAuthenticated()
    {
        var topicId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var expected = new Topic();
        topicRepository
            .Setup(r => r.Get(It.IsAny<Guid>(), It.IsAny<ForumAccessPolicy>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        accessPolicyConverter
            .Setup(c => c.Convert(It.IsAny<UserRole>()))
            .Returns(ForumAccessPolicy.Player);
        currentUserSetup.Returns(Create.User(userId).WithRole(UserRole.Player).Please);
        unreadCountersRepository
            .Setup(r => r.SelectByEntities(It.IsAny<Guid>(), It.IsAny<UnreadEntryType>(), It.IsAny<IReadOnlyCollection<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<Guid, int>{[topicId] = 22});

        var actual = await service.GetTopic(topicId, CancellationToken.None);

        actual.Should().Be(expected);
        actual.UnreadCommentsCount.Should().Be(22);
        unreadCountersRepository.Verify(r => r.SelectByEntities(userId, UnreadEntryType.Message, new[] {topicId}, It.IsAny<CancellationToken>()), Times.Once);
    }
}