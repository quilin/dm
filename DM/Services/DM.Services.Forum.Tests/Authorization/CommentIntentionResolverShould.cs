using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.Dto.Output;
using DM.Services.Forum.Tests.Dsl;
using FluentAssertions;
using Xunit;

namespace DM.Services.Forum.Tests.Authorization
{
    public class CommentIntentionResolverShould
    {
        private readonly CommentIntentionResolver resolver;

        public CommentIntentionResolverShould()
        {
            resolver = new CommentIntentionResolver();
        }

        [Theory]
        [InlineData(CommentIntention.Edit)]
        [InlineData(CommentIntention.Delete)]
        public async Task AllowEditingAndDeletingForAuthors(CommentIntention intention)
        {
            var userId = Guid.NewGuid();
            var author = Create.User(userId).WithRole(UserRole.Player).Please();
            var actual = await resolver.IsAllowed(author, intention, new Comment {Author = author});
            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData(CommentIntention.Edit)]
        [InlineData(CommentIntention.Delete)]
        public async Task NotAllowedEditingAndDeletingForUnauthenticated(CommentIntention intention)
        {
            var userId = Guid.NewGuid();
            var author = Create.User(userId).Please();
            var actual = await resolver.IsAllowed(author, intention, new Comment {Author = Create.User().Please()});
            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData(CommentIntention.Edit)]
        [InlineData(CommentIntention.Delete)]
        public async Task NotAllowEditingAndDeletingForAuthors(CommentIntention intention)
        {
            var userId = Guid.NewGuid();
            var author = Create.User(userId).WithRole(UserRole.NannyModerator).Please();
            var actual = await resolver.IsAllowed(author, intention, new Comment {Author = Create.User().Please()});
            actual.Should().BeFalse();
        }

        [Fact]
        public async Task NotAllowLikeForGuest()
        {
            var user = Create.User().WithRole(UserRole.Guest).Please();
            var actual = await resolver.IsAllowed(user, CommentIntention.Like, new Comment());
            actual.Should().BeFalse();
        }

        [Fact]
        public async Task NotAllowLikeOwnComments()
        {
            var userId = Guid.NewGuid();
            var user = Create.User(userId).WithRole(UserRole.Player).Please();
            var actual = await resolver.IsAllowed(user, CommentIntention.Like, new Comment {Author = user});
            actual.Should().BeFalse();
        }

        [Fact]
        public async Task AllowToLikeOthersComments()
        {
            var user = Create.User().WithRole(UserRole.Player).Please();
            var actual = await resolver.IsAllowed(user, CommentIntention.Like,
                new Comment {Author = Create.User().Please()});
            actual.Should().BeTrue();
        }
    }
}