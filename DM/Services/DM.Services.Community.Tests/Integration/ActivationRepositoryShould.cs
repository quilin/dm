using System;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Activation;
using DM.Services.DataAccess.BusinessObjects.Users;
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
                    Type = TokenType.Registration,
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
    }
}