using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Games;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto;
using DM.Tests.Core;
using FluentAssertions;
using Xunit;

namespace DM.Services.Gaming.Tests
{
    public class GameReadingRepositoryShould : DbTestBase
    {
        [Fact]
        public async Task MapCorrectly()
        {
            using (var rdb = GetRdb(nameof(MapCorrectly)))
            {
                var myUserId = Guid.NewGuid();
                var anotherUserId = Guid.NewGuid();
                var gameId1 = Guid.NewGuid();
                var gameId2 = Guid.NewGuid();
                rdb.Users.AddRange(new User
                {
                    UserId = myUserId,
                    Login = "login",
                    Email = "test@email.com"
                }, new User
                {
                    UserId = anotherUserId,
                    Location = "anotherLogin",
                    Email = "another@mail.com"
                });
                rdb.Games.AddRange(new Game
                {
                    GameId = Guid.NewGuid(),
                    MasterId = myUserId,
                    Title = "game1",
                    SystemName = "s",
                    SettingName = "s"
                }, new Game
                {
                    GameId = Guid.NewGuid(),
                    MasterId = anotherUserId,
                    AssistantId = myUserId,
                    Title = "game2",
                    SystemName = "s",
                    SettingName = "s"
                }, new Game
                {
                    GameId = Guid.NewGuid(),
                    MasterId = anotherUserId,
                    NannyId = myUserId,
                    Title = "game4",
                    SettingName = "s",
                    SystemName = "s"
                }, new Game
                {
                    GameId = gameId1,
                    MasterId = anotherUserId,
                    Title = "game5",
                    SettingName = "s",
                    SystemName = "s"
                }, new Game
                {
                    GameId = gameId2,
                    MasterId = anotherUserId,
                    Title = "game6",
                    SettingName = "s",
                    SystemName = "s"
                }, new Game
                {
                    GameId = Guid.NewGuid(),
                    MasterId = anotherUserId,
                    Title = "game7",
                    SettingName = "s",
                    SystemName = "s"
                });
                rdb.Characters.AddRange(new Character
                {
                    CharacterId = Guid.NewGuid(),
                    UserId = myUserId,
                    GameId = gameId1,
                    Name = "character1",
                    Status = CharacterStatus.Active
                }, new Character
                {
                    CharacterId = Guid.NewGuid(),
                    UserId = anotherUserId,
                    GameId = gameId1,
                    Name = "character2",
                    Status = CharacterStatus.Active
                }, new Character
                {
                    CharacterId = Guid.NewGuid(),
                    UserId = myUserId,
                    GameId = gameId2,
                    Name = "character3",
                    Status = CharacterStatus.Declined
                });

                await rdb.SaveChangesAsync();

                var gameReadingRepository = new GameReadingRepository(rdb, GetMapper());
                var games = await gameReadingRepository.GetGames(new PagingData(new PagingQuery
                    {
                        Size = 10,
                        Skip = 0
                    }, 10, 7), null,
                    myUserId);
                var gamesIndex = games.ToDictionary(g => g.Id);

                gamesIndex[gameId2].Participation(myUserId).Should().Be(GameParticipation.None);
                gamesIndex.Remove(gameId2);
                gamesIndex.Values.Select(v => v.Participation(myUserId)).Should().NotContain(GameParticipation.None);
            }
        }
    }
}