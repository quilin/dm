using System;
using DM.Services.Authentication.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.Dto.Output;
using DM.Services.Forum.Tests.Dsl;
using FluentAssertions;
using Xunit;

namespace DM.Services.Forum.Tests.Authorization;

public class TopicIntentionResolverShould
{
    private readonly TopicIntentionResolver resolver = new();

    [Theory]
    [InlineData(TopicIntention.Like)]
    [InlineData(TopicIntention.CreateComment)]
    [InlineData(TopicIntention.Edit)]
    public void ForbidEverythingForGuest(TopicIntention intention)
    {
        resolver.IsAllowed(AuthenticatedUser.Guest, intention, new Topic()).Should().BeFalse();
    }

    [Fact]
    public void ForbidCreateCommentInClosedTopic()
    {
        var actual = resolver.IsAllowed(
            Create.User().WithRole(UserRole.Player).Please(),
            TopicIntention.CreateComment,
            new Topic
            {
                Closed = true
            });
        actual.Should().BeFalse();
    }

    [Fact]
    public void AllowCreateCommentInOpenTopic()
    {
        var actual = resolver.IsAllowed(
            Create.User().WithRole(UserRole.Player).Please(),
            TopicIntention.CreateComment,
            new Topic
            {
                Closed = false
            });
        actual.Should().BeTrue();
    }

    [Fact]
    public void ForbidEditWhenUserNotAuthorAndNotLocalModeratorAndNotAdministrator()
    {
        var actual = resolver.IsAllowed(
            Create.User().WithRole(UserRole.RegularModerator).Please(),
            TopicIntention.Edit,
            new Topic
            {
                Author = Create.User().Please(),
                Closed = false,
                Forum = new Dto.Output.Forum
                {
                    ModeratorIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
                }
            });
        actual.Should().BeFalse();
    }

    [Fact]
    public void AllowEditWhenUserIsAuthor()
    {
        var userId = Guid.NewGuid();
        var actual = resolver.IsAllowed(
            Create.User(userId).WithRole(UserRole.RegularModerator).Please(),
            TopicIntention.Edit,
            new Topic
            {
                Author = Create.User(userId).Please(),
                Closed = false,
                Forum = new Dto.Output.Forum
                {
                    ModeratorIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
                }
            });
        actual.Should().BeTrue();
    }

    [Fact]
    public void AllowEditWhenUserIsLocalModerator()
    {
        var userId = Guid.NewGuid();
        var actual = resolver.IsAllowed(
            Create.User(userId).WithRole(UserRole.RegularModerator).Please(),
            TopicIntention.Edit,
            new Topic
            {
                Author = Create.User().Please(),
                Closed = false,
                Forum = new Dto.Output.Forum
                {
                    ModeratorIds = new[] {userId, Guid.NewGuid()}
                }
            });
        actual.Should().BeTrue();
    }

    [Fact]
    public void AllowEditWhenUserIsAdministrator()
    {
        var actual = resolver.IsAllowed(
            Create.User().WithRole(UserRole.Administrator).Please(),
            TopicIntention.Edit,
            new Topic
            {
                Author = Create.User().Please(),
                Closed = false,
                Forum = new Dto.Output.Forum
                {
                    ModeratorIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
                }
            });
        actual.Should().BeTrue();
    }

    [Fact]
    public void ForbidLikeWhenUserIsAuthor()
    {
        var userId = Guid.NewGuid();
        var actual = resolver.IsAllowed(
            Create.User(userId).WithRole(UserRole.Administrator).Please(),
            TopicIntention.Like,
            new Topic
            {
                Author = Create.User(userId).Please(),
                Closed = false,
                Forum = new Dto.Output.Forum
                {
                    ModeratorIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
                }
            });
        actual.Should().BeFalse();
    }

    [Fact]
    public void ForbidLikeWhenUserIsNotAuthor()
    {
        var actual = resolver.IsAllowed(
            Create.User().WithRole(UserRole.Administrator).Please(),
            TopicIntention.Like,
            new Topic
            {
                Author = Create.User().Please(),
                Closed = false,
                Forum = new Dto.Output.Forum
                {
                    ModeratorIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
                }
            });
        actual.Should().BeTrue();
    }
}