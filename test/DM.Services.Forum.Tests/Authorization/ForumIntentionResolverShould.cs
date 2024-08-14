using System;
using DM.Services.Authentication.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Common;
using DM.Services.Forum.Tests.Dsl;
using DM.Tests.Core;
using FluentAssertions;
using Moq;
using Xunit;

namespace DM.Services.Forum.Tests.Authorization;

public class ForumIntentionResolverShould : UnitTestBase
{
    private readonly Mock<IAccessPolicyConverter> policyConverter;
    private readonly ForumIntentionResolver resolver;

    public ForumIntentionResolverShould()
    {
        policyConverter = Mock<IAccessPolicyConverter>();
        resolver = new ForumIntentionResolver(policyConverter.Object);
    }

    [Fact]
    public void ForbidCreateTopicWhenCreatePolicyMatchesNotUserRole()
    {
        policyConverter
            .Setup(c => c.Convert(UserRole.Administrator))
            .Returns(
                ForumAccessPolicy.ForumModerator |
                ForumAccessPolicy.Player |
                ForumAccessPolicy.NannyModerator);
            
        var actual = resolver.IsAllowed(
            Create.User().WithRole(UserRole.Administrator).Please(),
            ForumIntention.CreateTopic,
            new Dto.Output.Forum
            {
                CreateTopicPolicy = ForumAccessPolicy.Guest | ForumAccessPolicy.SeniorModerator
            });
        actual.Should().BeFalse();
    }

    [Fact]
    public void ForbidCreateTopicWhenUserNotAuthenticated()
    {
        resolver.IsAllowed(
                Create.User().Please(),
                ForumIntention.CreateTopic,
                new Dto.Output.Forum())
            .Should().BeFalse();
    }

    [Fact]
    public void AllowCreateTopicWhenCreatePolicyMatchUserRole()
    {
        policyConverter
            .Setup(c => c.Convert(UserRole.Administrator | UserRole.Player))
            .Returns(
                ForumAccessPolicy.Guest |
                ForumAccessPolicy.Player |
                ForumAccessPolicy.Administrator);
            
        var actual = resolver.IsAllowed(
            Create.User().WithRole(UserRole.Administrator | UserRole.Player).Please(),
            ForumIntention.CreateTopic,
            new Dto.Output.Forum
            {
                CreateTopicPolicy = ForumAccessPolicy.Administrator | ForumAccessPolicy.SeniorModerator
            });
        actual.Should().BeTrue();
    }

    [Fact]
    public void ForbidTopicAdministrationWhenUserNotAdministratorOrLocalModerator()
    {
        var actual = resolver.IsAllowed(
            Create.User().WithRole(UserRole.Player | UserRole.RegularModerator).Please(),
            ForumIntention.AdministrateTopics,
            new Dto.Output.Forum
            {
                ModeratorIds = new [] {Guid.NewGuid(), Guid.NewGuid()}
            });
        actual.Should().BeFalse();
    }

    [Fact]
    public void AllowTopicAdministrationWhenUserAdministrator()
    {
        var actual = resolver.IsAllowed(
            Create.User().WithRole(UserRole.NannyModerator | UserRole.Administrator).Please(),
            ForumIntention.AdministrateTopics,
            new Dto.Output.Forum
            {
                ModeratorIds = new [] {Guid.NewGuid(), Guid.NewGuid()}
            });
        actual.Should().BeTrue();
    }

    [Fact]
    public void AllowTopicAdministrationWhenUserLocalModerator()
    {
        var userId = Guid.NewGuid();
        var actual = resolver.IsAllowed(
            Create.User(userId).WithRole(UserRole.NannyModerator | UserRole.SeniorModerator).Please(),
            ForumIntention.AdministrateTopics,
            new Dto.Output.Forum
            {
                ModeratorIds = new [] {Guid.NewGuid(), Guid.NewGuid(), userId}
            });
        actual.Should().BeTrue();
    }

    [Theory]
    [InlineData(ForumIntention.CreateTopic)]
    [InlineData(ForumIntention.AdministrateTopics)]
    public void AllowNothingWhenUserGuest(ForumIntention intention)
    {
        resolver.IsAllowed(AuthenticatedUser.Guest, intention, new Dto.Output.Forum()).Should().BeFalse();
    }
}