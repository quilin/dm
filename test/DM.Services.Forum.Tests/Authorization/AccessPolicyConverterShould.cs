using DM.Services.Core.Dto.Enums;
using DM.Services.Forum.BusinessProcesses.Common;
using FluentAssertions;
using Xunit;

namespace DM.Services.Forum.Tests.Authorization;

public class AccessPolicyConverterShould
{
    private readonly AccessPolicyConverter converter = new();

    [Fact]
    public void ReturnGuestPolicyForGuestUser()
    {
        converter.Convert(UserRole.Guest).Should().Be(ForumAccessPolicy.Guest);
    }

    [Theory]
    [InlineData(UserRole.Player, ForumAccessPolicy.Player)]
    [InlineData(UserRole.Administrator, ForumAccessPolicy.Administrator)]
    [InlineData(UserRole.SeniorModerator, ForumAccessPolicy.SeniorModerator)]
    [InlineData(UserRole.RegularModerator, ForumAccessPolicy.RegularModerator)]
    [InlineData(UserRole.NannyModerator, ForumAccessPolicy.NannyModerator)]
    public void MapRolesAndPoliciesAccordingly(UserRole role, ForumAccessPolicy policy)
    {
        converter.Convert(role).Should().HaveFlag(policy);
    }

    [Theory]
    [InlineData(UserRole.Player)]
    [InlineData(UserRole.SeniorModerator)]
    [InlineData(UserRole.Administrator | UserRole.NannyModerator)]
    [InlineData(UserRole.RegularModerator | UserRole.Player)]
    public void ContainGuestAccessForNonGuestUsers(UserRole role)
    {
        converter.Convert(role).Should().HaveFlag(ForumAccessPolicy.Guest);
    }
}