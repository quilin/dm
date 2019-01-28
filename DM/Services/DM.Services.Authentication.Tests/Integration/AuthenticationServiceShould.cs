using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Factories;
using DM.Services.Authentication.Implementation;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Repositories;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.MongoIntegration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Xunit;

namespace DM.Services.Authentication.Tests.Integration
{
    public class AuthenticationServiceShould
    {
        private readonly AuthenticationService authenticationService;
        private readonly DmMongoClient dmMongoClient;

        public AuthenticationServiceShould()
        {
            dmMongoClient = new DmMongoClient("mongodb://localhost:27017/dm3release?maxPoolSize=1000");
            authenticationService = new AuthenticationService(
                new SecurityManager(new SaltFactory(), new HashProvider()),
                new TripleDesSymmetricCryptoService(),
                new AuthenticationRepository(
                    new DmDbContext(new DbContextOptionsBuilder<DmDbContext>()
                        .UseNpgsql(
                            "User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=dm3;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection Idle Lifetime=60;")
                        .Options),
                    dmMongoClient),
                new SessionFactory(new GuidFactory(), new DateTimeProvider()),
                new DateTimeProvider());
        }

        [Fact]
        public async Task CreateNewSessionIfNotPresent()
        {
            var actual = await authenticationService.Authenticate("Quilin", "gVczyBCtus5m", true);

            actual.Error.Should().Be(AuthenticationError.NoError);

            var cursor = await dmMongoClient.GetCollection<UserSessions>()
                .FindAsync(new FilterDefinitionBuilder<UserSessions>()
                    .ElemMatch(u => u.Sessions, s => s.Id == actual.Session.Id));
            var userSessions = await cursor.FirstAsync();

            userSessions.Id.Should().Be(actual.User.UserId);
            userSessions.Sessions.Should().HaveCount(1);
            userSessions.Sessions.First().Should().BeEquivalentTo(actual.Session, options => options
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                .When(info => info.SelectedMemberInfo.MemberType == typeof(DateTime)));
        }
    }
}