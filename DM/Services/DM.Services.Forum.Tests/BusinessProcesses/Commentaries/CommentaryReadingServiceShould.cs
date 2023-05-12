using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.BusinessProcesses.Commentaries.Reading;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.Dto.Output;
using DM.Services.Forum.Tests.Dsl;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Xunit;
using Comment = DM.Services.Common.Dto.Comment;

namespace DM.Services.Forum.Tests.BusinessProcesses.Commentaries;

public class CommentaryReadingServiceShould
{
    private readonly ISetup<ITopicReadingService, Task<Topic>> readingTopicSetup;
    private readonly ISetup<ICommentaryReadingRepository, Task<IEnumerable<Comment>>> getCommentsListSetup;
    private readonly ISetup<ICommentaryReadingRepository, Task<Comment>> getCommentSetup;
    private readonly ISetup<ICommentaryReadingRepository, Task<int>> countCommentsSetup;
    private readonly ISetup<IIdentity, AuthenticatedUser> currentUserSetup;
    private readonly Mock<IUnreadCountersRepository> unreadCountersRepository;
    private readonly ISetup<IForumReadingService, Task<Dto.Output.Forum>> getForumSetup;
    private readonly CommentaryReadingService readingService;

    public CommentaryReadingServiceShould()
    {
        var topicReadingService = new Mock<ITopicReadingService>();
        readingTopicSetup = topicReadingService.Setup(r => r.GetTopic(It.IsAny<Guid>()));

        var identity = new Mock<IIdentity>();
        identity.Setup(i => i.Settings).Returns(new UserSettings
            {Paging = new PagingSettings {CommentsPerPage = 10}});
        currentUserSetup = identity.Setup(i => i.User);
        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);

        var commentaryRepository = new Mock<ICommentaryReadingRepository>();
        getCommentsListSetup = commentaryRepository.Setup(r => r.Get(It.IsAny<Guid>(), It.IsAny<PagingData>()));
        getCommentSetup = commentaryRepository.Setup(r => r.Get(It.IsAny<Guid>()));
        countCommentsSetup = commentaryRepository.Setup(r => r.Count(It.IsAny<Guid>()));

        var forumReadingService = new Mock<IForumReadingService>();
        getForumSetup = forumReadingService.Setup(s => s.GetForum(It.IsAny<string>(), It.IsAny<bool>()));

        unreadCountersRepository = new Mock<IUnreadCountersRepository>();
        unreadCountersRepository
            .Setup(r => r.Flush(It.IsAny<Guid>(), It.IsAny<UnreadEntryType>(), It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);
        unreadCountersRepository
            .Setup(r => r.FlushAll(It.IsAny<Guid>(), It.IsAny<UnreadEntryType>(), It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        readingService = new CommentaryReadingService(topicReadingService.Object,
            forumReadingService.Object, identityProvider.Object,
            unreadCountersRepository.Object, commentaryRepository.Object);
    }

    [Fact]
    public void ThrowException_WhenNothingFound()
    {
        var commentId = Guid.NewGuid();
        getCommentSetup.ReturnsAsync((Comment) null);
        readingService.Invoking(s => s.Get(commentId).Wait())
            .Should().Throw<HttpException>().And
            .StatusCode.Should().Be(HttpStatusCode.Gone);
    }

    [Fact]
    public async Task ReturnFoundCommentary()
    {
        var expected = new Comment();
        getCommentSetup.ReturnsAsync(expected);
        var actual = await readingService.Get(Guid.NewGuid());
        actual.Should().Be(expected);
    }

    [Fact]
    public async Task ReturnFoundList()
    {
        var topicId = Guid.NewGuid();
        var expected = new[] {new Comment(), new Comment()};
        readingTopicSetup.ReturnsAsync(new Topic {Id = topicId});
        countCommentsSetup.ReturnsAsync(20);
        getCommentsListSetup.ReturnsAsync(expected);
        currentUserSetup.Returns(Create.User().WithRole(UserRole.Guest).Please);

        var (actualList, actualPaging) = await readingService.Get(topicId, new PagingQuery());

        actualList.Should().BeSameAs(expected);
        actualPaging.Should().NotBeNull();
    }

    [Fact]
    public async Task FlushTopicCounter()
    {
        var topicId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        readingTopicSetup.ReturnsAsync(new Topic());
        currentUserSetup.Returns(Create.User(userId).Please);

        await readingService.MarkAsRead(topicId);

        unreadCountersRepository.Verify(r => r.Flush(userId, UnreadEntryType.Message, topicId), Times.Once);
        unreadCountersRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task FlushForumCounters()
    {
        var forumId = "Forum";
        var forumInternalId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        currentUserSetup.Returns(Create.User(userId).Please);
        getForumSetup.ReturnsAsync(new Dto.Output.Forum {Id = forumInternalId});

        await readingService.MarkAsRead(forumId);

        unreadCountersRepository.Verify(r => r.FlushAll(userId, UnreadEntryType.Message, forumInternalId),
            Times.Once);
        unreadCountersRepository.VerifyNoOtherCalls();
    }
}