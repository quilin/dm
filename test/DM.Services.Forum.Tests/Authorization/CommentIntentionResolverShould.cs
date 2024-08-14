using System;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.Tests.Dsl;
using FluentAssertions;
using Xunit;

namespace DM.Services.Forum.Tests.Authorization;

public class CommentIntentionResolverShould
{
    private readonly CommentIntentionResolver resolver = new();

    [Theory]
    [InlineData(CommentIntention.Edit)]
    [InlineData(CommentIntention.Delete)]
    public void AllowEditingAndDeletingForAuthors(CommentIntention intention)
    {
        var userId = Guid.NewGuid();
        var author = Create.User(userId).WithRole(UserRole.Player).Please();
        var actual = resolver.IsAllowed(author, intention, new Comment {Author = author});
        actual.Should().BeTrue();
    }

    [Theory]
    [InlineData(CommentIntention.Edit)]
    [InlineData(CommentIntention.Delete)]
    public void NotAllowedEditingAndDeletingForUnauthenticated(CommentIntention intention)
    {
        var userId = Guid.NewGuid();
        var author = Create.User(userId).Please();
        var actual = resolver.IsAllowed(author, intention, new Comment {Author = Create.User().Please()});
        actual.Should().BeFalse();
    }

    [Theory]
    [InlineData(CommentIntention.Edit)]
    [InlineData(CommentIntention.Delete)]
    public void NotAllowEditingAndDeletingForAuthors(CommentIntention intention)
    {
        var userId = Guid.NewGuid();
        var author = Create.User(userId).WithRole(UserRole.NannyModerator).Please();
        var actual = resolver.IsAllowed(author, intention, new Comment {Author = Create.User().Please()});
        actual.Should().BeFalse();
    }

    [Fact]
    public void NotAllowLikeForGuest()
    {
        var user = Create.User().WithRole(UserRole.Guest).Please();
        var actual = resolver.IsAllowed(user, CommentIntention.Like, new Comment());
        actual.Should().BeFalse();
    }

    [Fact]
    public void NotAllowLikeOwnComments()
    {
        var userId = Guid.NewGuid();
        var user = Create.User(userId).WithRole(UserRole.Player).Please();
        var actual = resolver.IsAllowed(user, CommentIntention.Like, new Comment {Author = user});
        actual.Should().BeFalse();
    }

    [Fact]
    public void AllowToLikeOthersComments()
    {
        var user = Create.User().WithRole(UserRole.Player).Please();
        var actual = resolver.IsAllowed(user, CommentIntention.Like, new Comment {Author = Create.User().Please()});
        actual.Should().BeTrue();
    }
}