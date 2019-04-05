using DM.Services.Core.Dto.Enums;
using DM.Services.Forum.Implementation;
using FluentAssertions;
using Xunit;

namespace DM.Services.Forum.Tests
{
    public class AccessPolicyConverterShould
    {
        private readonly AccessPolicyConverter converter;

        public AccessPolicyConverterShould()
        {
            converter = new AccessPolicyConverter();
        }

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
    }
}