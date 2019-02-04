using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Common.Repositories;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.Forum.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DM.Services.Forum.Tests
{
    public class ForumRepositoryShould
    {
        private readonly ForumRepository repository;

        public ForumRepositoryShould()
        {
            repository = new ForumRepository(
                new DmDbContext(new DbContextOptionsBuilder<DmDbContext>()
                    .UseNpgsql(
                        "User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=dm3;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection Idle Lifetime=60;")
                    .Options),
                new UnreadCountersRepository(
                    new DmMongoClient("mongodb://localhost:27017/dm3release?maxPoolSize=1000")));
        }
        
        [Fact]
        public async Task ReturnGuestAvailableFora()
        {
            var actual = (await repository.SelectFora(Guid.Empty, ForumAccessPolicy.Everyone)).ToArray();
            actual.Should().Contain(i => i.Title == "Общий");
            actual.Should().Contain(i => i.Title == "Игровые системы");
            actual.Should().Contain(i => i.Title == "Набор игроков и поиск мастера");
            actual.Should().Contain(i => i.Title == "Под столом");
            actual.Should().Contain(i => i.Title == "Улучшение сайта");
            actual.Should().Contain(i => i.Title == "Ошибки");
            actual.Should().Contain(i => i.Title == "Для новичков");
            actual.Should().Contain(i => i.Title == "Новости проекта");
        }

        [Fact]
        public async Task ReturnAdminAvailableFora()
        {
            var actual = (await repository.SelectFora(Guid.Empty, ForumAccessPolicy.Administrators)).ToArray();
            actual.Should().Contain(i => i.Title == "Питомник");
            actual.Should().Contain(i => i.Title == "Администрация");
            actual.Should().Contain(i => i.Title == "Пещера троллей");
        }
    }
}