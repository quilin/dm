using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Repositories;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Tests.Core;
using FluentAssertions;
using Xunit;

namespace DM.Services.Authentication.Tests.Integration
{
    public class AuthenticationRepositoryTryFindShould : DbTestBase
    {
        [Fact]
        public async Task ReturnFalseAndNullWhenNoMatchingUser()
        {
            using (var rdb = GetRdb(nameof(ReturnFalseAndNullWhenNoMatchingUser)))
            {
                rdb.Users.Add(new User
                {
                    UserId = Guid.NewGuid(),
                    Login = "Some user",
                    Email = "user@email.com",
                    RegistrationDate = DateTimeOffset.UtcNow,
                    Role = UserRole.Player,
                    Salt = "salt",
                    PasswordHash = "hash",
                    Activated = true,
                    IsRemoved = false
                });
                await rdb.SaveChangesAsync();

                var authenticationRepository = new AuthenticationRepository(rdb, null, GetMapper());
                var actual = await authenticationRepository.TryFindUser("Another user");

                actual.Success.Should().BeFalse();
                actual.User.Should().BeNull();
            }
        }

        [Fact]
        public async Task ReturnTrueAndUserWhenMatchingUser()
        {
            using (var rdb = GetRdb(nameof(ReturnTrueAndUserWhenMatchingUser)))
            {
                var userId = Guid.NewGuid();
                rdb.Users.Add(new User
                {
                    UserId = userId,
                    Login = "TheUser",
                    Email = "user@email.com",
                    RegistrationDate = DateTimeOffset.UtcNow,
                    Role = UserRole.Player,
                    Salt = "salt",
                    PasswordHash = "hash",
                    Activated = true,
                    IsRemoved = false
                });
                await rdb.SaveChangesAsync();

                var authenticationRepository = new AuthenticationRepository(rdb, null, GetMapper());
                var actual = await authenticationRepository.TryFindUser("tHeuSer");

                actual.Success.Should().BeTrue();
                actual.User.UserId.Should().Be(userId);
            }
        }
    }
}