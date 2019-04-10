using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Common;
using DM.Tests.Core;
using FluentAssertions;
using Moq;
using Xunit;

namespace DM.Services.Forum.Tests.Authorization
{
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
        public async Task ForbidCreateTopicWhenCreatePolicyMatchesNotUserRole()
        {
            policyConverter
                .Setup(c => c.Convert(UserRole.Administrator))
                .Returns(
                    ForumAccessPolicy.ForumModerator |
                    ForumAccessPolicy.Player |
                    ForumAccessPolicy.NannyModerator);
            
            var actual = await resolver.IsAllowed(
                new AuthenticatedUser {Role = UserRole.Administrator},
                ForumIntention.CreateTopic,
                new Dto.Output.Forum
            {
                CreateTopicPolicy = ForumAccessPolicy.Guest | ForumAccessPolicy.SeniorModerator
            });
            actual.Should().BeFalse();
        }

        [Fact]
        public async Task AllowCreateTopicWhenCreatePolicyMatchUserRole()
        {
            policyConverter
                .Setup(c => c.Convert(UserRole.Administrator | UserRole.Player))
                .Returns(
                    ForumAccessPolicy.Guest |
                    ForumAccessPolicy.Player |
                    ForumAccessPolicy.Administrator);
            
            var actual = await resolver.IsAllowed(
                new AuthenticatedUser {Role = UserRole.Administrator | UserRole.Player},
                ForumIntention.CreateTopic,
                new Dto.Output.Forum
                {
                    CreateTopicPolicy = ForumAccessPolicy.Administrator | ForumAccessPolicy.SeniorModerator
                });
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task ForbidTopicAdministrationWhenUserNotAdministratorOrLocalModerator()
        {
            var actual = await resolver.IsAllowed(
                new AuthenticatedUser
                {
                    Role = UserRole.Player | UserRole.RegularModerator,
                    UserId = Guid.NewGuid()
                },
                ForumIntention.AdministrateTopics,
                new Dto.Output.Forum
                {
                    ModeratorIds = new [] {Guid.NewGuid(), Guid.NewGuid()}
                });
            actual.Should().BeFalse();
        }

        [Fact]
        public async Task AllowTopicAdministrationWhenUserAdministrator()
        {
            var actual = await resolver.IsAllowed(
                new AuthenticatedUser
                {
                    Role = UserRole.NannyModerator | UserRole.Administrator,
                    UserId = Guid.NewGuid()
                },
                ForumIntention.AdministrateTopics,
                new Dto.Output.Forum
                {
                    ModeratorIds = new [] {Guid.NewGuid(), Guid.NewGuid()}
                });
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task AllowTopicAdministrationWhenUserLocalModerator()
        {
            var userId = Guid.NewGuid();
            var actual = await resolver.IsAllowed(
                new AuthenticatedUser
                {
                    Role = UserRole.NannyModerator | UserRole.SeniorModerator,
                    UserId = userId
                },
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
        public async Task AllowNothingWhenUserGuest(ForumIntention intention)
        {
            (await resolver.IsAllowed(AuthenticatedUser.Guest, intention, new Dto.Output.Forum())).Should().BeFalse();
        }
    }
}