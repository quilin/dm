using DM.Services.Authentication.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Tests.Core;
using FluentAssertions;
using Xunit;

namespace DM.Services.Authentication.Tests;

public class AuthenticatedUserShould : UnitTestBase
{
    [Fact]
    public void ComputeGeneralAccessPolicy()
    {
        var authenticatedUser = new AuthenticatedUser
        {
            AccessRestrictionPolicies = new[]
            {
                AccessPolicy.ChatBan,
                AccessPolicy.DemocraticBan,
                AccessPolicy.RestrictContentEditing
            },
            AccessPolicy = AccessPolicy.DemocraticBan
        };
        var actual = authenticatedUser.GeneralAccessPolicy;
        actual.Should().Be(
            AccessPolicy.ChatBan |
            AccessPolicy.DemocraticBan |
            AccessPolicy.RestrictContentEditing);
    }
}