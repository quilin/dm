using System;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Account.Activation;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using DM.Tests.Core;
using FluentAssertions;
using Xunit;

namespace DM.Services.Community.Tests.Integration
{
    public class ActivationRepositoryShould : DbTestBase
    {
        [Fact]
        public async Task FindUserToActivate()
        {
            using (var rdb = GetRdb(nameof(FindUserToActivate)))
            {
                var userId1 = Guid.NewGuid();
                rdb.Users.Add(new User
                {
                    UserId = userId1,
                    Login = "login",
                    Email = "some@email.ru",
                    Activated = false,
                    IsRemoved = false
                });
                var tokenId1 = Guid.NewGuid();
                rdb.Tokens.Add(new Token
                {
                    TokenId = tokenId1,
                    Type = TokenType.Activation,
                    UserId = userId1,
                    IsRemoved = false,
                    CreateDate = DateTimeOffset.Now
                });
                await rdb.SaveChangesAsync();

                var activationRepository = new ActivationRepository(rdb);

                (await activationRepository.FindUserToActivate(tokenId1, DateTimeOffset.Now.AddDays(-1)))
                    .Should().Be(userId1);
                (await activationRepository.FindUserToActivate(Guid.NewGuid(), DateTimeOffset.Now))
                    .Should().BeNull();
                (await activationRepository.FindUserToActivate(tokenId1, DateTimeOffset.Now.AddDays(1)))
                    .Should().BeNull();
            }
        }

        [Fact]
        public async Task ActivateUser()
        {
            var userId = Guid.NewGuid();
            var tokenId = Guid.NewGuid();
            using (var rdb = GetRdb(nameof(ActivateUser)))
            {
                rdb.Users.Add(new User
                {
                    UserId = userId,
                    Login = "login",
                    Email = "some@email.ru",
                    Activated = false,
                    IsRemoved = false
                });
                rdb.Tokens.Add(new Token
                {
                    TokenId = tokenId,
                    Type = TokenType.Activation,
                    UserId = userId,
                    IsRemoved = false,
                    CreateDate = DateTimeOffset.Now
                });
                await rdb.SaveChangesAsync();
            }

            using (var rdb = GetRdb(nameof(ActivateUser)))
            {
                var activationRepository = new ActivationRepository(rdb);
                var userUpdate = new UpdateBuilderFactory().Create<User>(userId)
                    .Field(u => u.Activated, true);
                var tokenUpdate = new UpdateBuilderFactory().Create<Token>(tokenId)
                    .Field(t => t.IsRemoved, true);
                await activationRepository.ActivateUser(userUpdate, tokenUpdate);

                (await rdb.Tokens.FindAsync(tokenId)).IsRemoved.Should().BeTrue();
                (await rdb.Users.FindAsync(userId)).Activated.Should().BeTrue();
            }
        }
    }
}