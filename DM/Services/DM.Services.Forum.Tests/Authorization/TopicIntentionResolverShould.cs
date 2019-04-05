using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.Dto;
using FluentAssertions;
using Xunit;

namespace DM.Services.Forum.Tests.Authorization
{
    public class TopicIntentionResolverShould
    {
        private readonly TopicIntentionResolver resolver;

        public TopicIntentionResolverShould()
        {
            resolver = new TopicIntentionResolver();
        }

        [Theory]
        [InlineData(TopicIntention.Like)]
        [InlineData(TopicIntention.CreateComment)]
        [InlineData(TopicIntention.Edit)]
        public async Task ForbidEverythingForGuest(TopicIntention intention)
        {
            (await resolver.IsAllowed(AuthenticatedUser.Guest, intention, new Topic())).Should().BeFalse();
        }

        [Fact]
        public async Task ForbidCreateCommentInClosedTopic()
        {
            var actual = await resolver.IsAllowed(new AuthenticatedUser {Role = UserRole.Player},
                TopicIntention.CreateComment,
                new Topic
                {
                    Closed = true
                });
            actual.Should().BeFalse();
        }

        [Fact]
        public async Task AllowCreateCommentInOpenTopic()
        {
            var actual = await resolver.IsAllowed(new AuthenticatedUser {Role = UserRole.Player},
                TopicIntention.CreateComment,
                new Topic
                {
                    Closed = false
                });
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task ForbidEditWhenUserNotAuthorAndNotLocalModeratorAndNotAdministrator()
        {
            var actual = await resolver.IsAllowed(
                new AuthenticatedUser
                {
                    UserId = Guid.NewGuid(),
                    Role = UserRole.RegularModerator
                },
                TopicIntention.Edit,
                new Topic
                {
                    Author = new GeneralUser{UserId = Guid.NewGuid()},
                    Closed = false,
                    Forum = new Dto.Forum
                    {
                        ModeratorIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
                    }
                });
            actual.Should().BeFalse();
        }

        [Fact]
        public async Task AllowEditWhenUserIsAuthor()
        {
            var userId = Guid.NewGuid();
            var actual = await resolver.IsAllowed(
                new AuthenticatedUser
                {
                    UserId = userId,
                    Role = UserRole.RegularModerator
                },
                TopicIntention.Edit,
                new Topic
                {
                    Author = new GeneralUser{UserId = userId},
                    Closed = false,
                    Forum = new Dto.Forum
                    {
                        ModeratorIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
                    }
                });
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task AllowEditWhenUserIsLocalModerator()
        {
            var userId = Guid.NewGuid();
            var actual = await resolver.IsAllowed(
                new AuthenticatedUser
                {
                    UserId = userId,
                    Role = UserRole.RegularModerator
                },
                TopicIntention.Edit,
                new Topic
                {
                    Author = new GeneralUser{UserId = Guid.NewGuid()},
                    Closed = false,
                    Forum = new Dto.Forum
                    {
                        ModeratorIds = new[] {userId, Guid.NewGuid()}
                    }
                });
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task AllowEditWhenUserIsAdministrator()
        {
            var actual = await resolver.IsAllowed(
                new AuthenticatedUser
                {
                    UserId = Guid.NewGuid(),
                    Role = UserRole.Administrator
                },
                TopicIntention.Edit,
                new Topic
                {
                    Author = new GeneralUser{UserId = Guid.NewGuid()},
                    Closed = false,
                    Forum = new Dto.Forum
                    {
                        ModeratorIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
                    }
                });
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task ForbidLikeWhenUserIsAuthor()
        {
            var userId = Guid.NewGuid();
            var actual = await resolver.IsAllowed(
                new AuthenticatedUser
                {
                    UserId = userId,
                    Role = UserRole.Administrator
                },
                TopicIntention.Like,
                new Topic
                {
                    Author = new GeneralUser{UserId = userId},
                    Closed = false,
                    Forum = new Dto.Forum
                    {
                        ModeratorIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
                    }
                });
            actual.Should().BeFalse();
        }

        [Fact]
        public async Task ForbidLikeWhenUserIsNotAuthor()
        {
            var actual = await resolver.IsAllowed(
                new AuthenticatedUser
                {
                    UserId = Guid.NewGuid(),
                    Role = UserRole.Administrator
                },
                TopicIntention.Like,
                new Topic
                {
                    Author = new GeneralUser{UserId = Guid.NewGuid()},
                    Closed = false,
                    Forum = new Dto.Forum
                    {
                        ModeratorIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
                    }
                });
            actual.Should().BeTrue();
        }
    }
}