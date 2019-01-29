using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Factories;
using DM.Services.Authentication.Implementation;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Repositories;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Testing.Core;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Xunit;
using Xunit.Abstractions;

namespace DM.Services.Authentication.Tests.Integration
{
    public class AuthenticationServiceShould : IntegrationTestBase
    {
        private readonly AuthenticationService authenticationService;

        public AuthenticationServiceShould(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            authenticationService = new AuthenticationService(
                new SecurityManager(new SaltFactory(), new HashProvider()),
                new TripleDesSymmetricCryptoService(),
                new AuthenticationRepository(DmDbContext, DmMongoClient),
                new SessionFactory(new GuidFactory(), new DateTimeProvider()),
                new DateTimeProvider());
        }

        [Fact]
        public async Task CreateNewSessionIfNotPresent()
        {
            var userId = await DmDbContext.Users.AsNoTracking()
                .Where(u => u.Login == "Quilin")
                .Select(u => u.UserId)
                .FirstAsync();
            await DmMongoClient.GetCollection<UserSessions>()
                .FindOneAndDeleteAsync(new FilterDefinitionBuilder<UserSessions>()
                    .Eq(s => s.Id, userId));
            
            var actual = await authenticationService.Authenticate("Quilin", "gVczyBCtus5m", true);

            actual.Error.Should().Be(AuthenticationError.NoError);

            var cursor = await DmMongoClient.GetCollection<UserSessions>()
                .FindAsync(new FilterDefinitionBuilder<UserSessions>()
                    .ElemMatch(u => u.Sessions, s => s.Id == actual.Session.Id));
            var userSessions = await cursor.FirstAsync();

            userSessions.Id.Should().Be(actual.User.UserId);
            userSessions.Sessions.Should().HaveCount(1);
            userSessions.Sessions.First().Should().BeEquivalentTo(actual.Session, options => options
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                .When(info => info.SelectedMemberInfo.MemberType == typeof(DateTime)));
        }

        [Fact]
        public async Task ReturnDecryptableToken()
        {
            var result = await authenticationService.Authenticate("Quilin", "gVczyBCtus5m", true);
            result.Error.Should().Be(AuthenticationError.NoError);

            var actual = await authenticationService.Authenticate(result.Token);
            actual.Error.Should().Be(AuthenticationError.NoError);
        }
    }
}