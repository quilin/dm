using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;
using DbGame = DM.Services.DataAccess.BusinessObjects.Games.Game;
using DbTag = DM.Services.DataAccess.BusinessObjects.Games.Links.GameTag;

namespace DM.Services.Gaming.BusinessProcesses.Games.Creating
{
    /// <inheritdoc />
    public class GameCreatingRepository : IGameCreatingRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public GameCreatingRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        
        /// <inheritdoc />
        public async Task<GameExtended> Create(DbGame game, DbRoom room,
            IEnumerable<DbTag> tags)
        {
            await Task.WhenAll(
                dbContext.Games.AddAsync(game),
                dbContext.Rooms.AddAsync(room),
                dbContext.GameTags.AddRangeAsync(tags));
            await dbContext.SaveChangesAsync();
            return await dbContext.Games
                .Where(g => g.GameId == game.GameId)
                .ProjectTo<GameExtended>(mapper.ConfigurationProvider)
                .FirstAsync();
        }

        /// <inheritdoc />
        public async Task<(bool exists, Guid userId)> FindUserId(string login)
        {
            var foundUserId = await dbContext.Users
                .Where(u => !u.IsRemoved && u.Activated && u.Login.ToLower() == login.ToLower())
                .Select(u => u.UserId)
                .FirstOrDefaultAsync();
            return (foundUserId != default, foundUserId);
        }
    }
}