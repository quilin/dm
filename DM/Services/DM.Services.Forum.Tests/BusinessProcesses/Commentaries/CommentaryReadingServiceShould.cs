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
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.Dto.Output;
using DM.Services.Forum.Tests.Dsl;
using DM.Tests.Core;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Forum.Tests.BusinessProcesses.Commentaries
{
    public class CommentaryReadingServiceShould : UnitTestBase
    {
        private readonly ISetup<ITopicReadingService, Task<Topic>> readingTopicSetup;
        private readonly ISetup<ICommentaryReadingRepository, Task<IEnumerable<Comment>>> getCommentsListSetup;
        private readonly ISetup<ICommentaryReadingRepository, Task<Comment>> getCommentSetup;
        private readonly ISetup<ICommentaryReadingRepository, Task<int>> countCommentsSetup;
        private readonly ISetup<IIdentity, AuthenticatedUser> currentUserSetup;
        private readonly Mock<IUnreadCountersRepository> unreadCountersRepository;
        private readonly CommentaryReadingService readingService;

        public CommentaryReadingServiceShould()
        {
            var topicReadingService = Mock<ITopicReadingService>();
            readingTopicSetup = topicReadingService.Setup(r => r.GetTopic(It.IsAny<Guid>()));

            var identity = Mock<IIdentity>();
            identity.Setup(i => i.Settings).Returns(new UserSettings {CommentsPerPage = 10});
            currentUserSetup = identity.Setup(i => i.User);
            var identityProvider = Mock<IIdentityProvider>();
            identityProvider.Setup(p => p.Current).Returns(identity.Object);

            var commentaryRepository = Mock<ICommentaryReadingRepository>();
            getCommentsListSetup = commentaryRepository.Setup(r => r.Get(It.IsAny<Guid>(), It.IsAny<PagingData>()));
            getCommentSetup = commentaryRepository.Setup(r => r.Get(It.IsAny<Guid>()));
            countCommentsSetup = commentaryRepository.Setup(r => r.Count(It.IsAny<Guid>()));

            unreadCountersRepository = Mock<IUnreadCountersRepository>();
            readingService = new CommentaryReadingService(topicReadingService.Object, identityProvider.Object,
                commentaryRepository.Object, unreadCountersRepository.Object);
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
            readingTopicSetup.ReturnsAsync(new Topic{Id = topicId});
            countCommentsSetup.ReturnsAsync(20);
            getCommentsListSetup.ReturnsAsync(expected);
            currentUserSetup.Returns(Create.User().WithRole(UserRole.Guest).Please);

            var (actualList, actualPaging) = await readingService.Get(topicId, new PagingQuery());
            
            actualList.Should().BeEquivalentTo((IEnumerable<Comment>) expected);
            actualPaging.Should().NotBeNull();
        }

        [Fact]
        public async Task FlushUnread_WhenUserAuthenticated()
        {
            var topicId = Guid.NewGuid();
            var expected = new[] {new Comment(), new Comment()};
            var userId = Guid.NewGuid();
            readingTopicSetup.ReturnsAsync(new Topic{Id = topicId});
            countCommentsSetup.ReturnsAsync(20);
            getCommentsListSetup.ReturnsAsync(expected);
            currentUserSetup.Returns(Create.User(userId).WithRole(UserRole.Player).Please);

            await readingService.Get(topicId, new PagingQuery());

            unreadCountersRepository.Verify(r => r.Flush(userId, UnreadEntryType.Message, topicId), Times.Once);
            unreadCountersRepository.VerifyNoOtherCalls();
        }
    }
}