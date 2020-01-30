using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Registration;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Tests.Core;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DM.Services.Community.Tests.Integration
{
    public class RegistrationRepositoryShould : DbTestBase
    {
        [Fact]
        public async Task CheckIfEmailIsFree()
        {
            using (var rdb = GetRdb(nameof(CheckIfEmailIsFree)))
            {
                rdb.Users.Add(new User
                {
                    UserId = Guid.NewGuid(),
                    Login = "login",
                    Email = "taken@email.ru"
                });
                await rdb.SaveChangesAsync();

                var registrationRepository = new RegistrationRepository(rdb);
                (await registrationRepository.EmailFree("taken@eMail.RU", CancellationToken.None))
                    .Should().BeFalse();
                (await registrationRepository.EmailFree("not_taken@email.com", CancellationToken.None))
                    .Should().BeTrue();
            }
        }
        
        [Fact]
        public async Task CheckIfLoginIsFree()
        {
            using (var rdb = GetRdb(nameof(CheckIfLoginIsFree)))
            {
                rdb.Users.Add(new User
                {
                    UserId = Guid.NewGuid(),
                    Login = "login_taken",
                    Email = "some@email.ru"
                });
                await rdb.SaveChangesAsync();

                var registrationRepository = new RegistrationRepository(rdb);
                (await registrationRepository.LoginFree("Another user69", CancellationToken.None))
                    .Should().BeTrue();
                (await registrationRepository.LoginFree("login_taken", CancellationToken.None))
                    .Should().BeFalse();
            }
        }

        [Fact]
        public async Task AddUser()
        {
            using (var rdb = GetRdb(nameof(CheckIfLoginIsFree)))
            {
                var registrationRepository = new RegistrationRepository(rdb);
                var userId = Guid.NewGuid();
                var user = new User
                {
                    UserId = userId,
                    Login = "test",
                    Email = "test@mail.ru"
                };
                var tokenId = Guid.NewGuid();
                var token = new Token
                {
                    UserId = userId,
                    Type = TokenType.Activation,
                    TokenId = tokenId,
                    CreateDate = DateTimeOffset.Now,
                    IsRemoved = false
                };
                await registrationRepository.AddUser(user, token);
                
                (await rdb.Users.FirstAsync(u => u.UserId == userId)).Should().BeEquivalentTo(user);
                (await rdb.Tokens.FirstAsync(t => t.TokenId == tokenId)).Should().BeEquivalentTo(token);
            }
        }
    }
}