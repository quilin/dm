using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Games.Reading
{
    /// <inheritdoc />
    public class GameReadingRepository : IGameReadingRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public GameReadingRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        private static Expression<Func<DataAccess.BusinessObjects.Games.Game, bool>> AccessFilter(Guid userId) =>
            g => !g.IsRemoved && g.BlackList.All(b => b.UserId != userId);

        /// <inheritdoc />
        public Task<int> Count(GameStatus? status, Guid userId)
        {
            return dbContext.Games
                .Where(AccessFilter(userId))
                .Where(g => !status.HasValue || g.Status == status.Value)
                .CountAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Game>> GetGames(PagingData pagingData, GameStatus? status, Guid userId)
        {
            return await dbContext.Games
                .Where(AccessFilter(userId))
                .Where(g => !status.HasValue || g.Status == status.Value)
                .OrderBy(g => g.ReleaseDate ?? g.CreateDate)
                .Skip(pagingData.Skip)
                .Take(pagingData.Take)
                .ProjectTo<Game>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        /// <inheritdoc />
        public Task<GameExtended> GetGameDetails(Guid gameId, Guid userId)
        {
            return dbContext.Games
                .Where(AccessFilter(userId))
                .Where(g => g.GameId == gameId)
                .ProjectTo<GameExtended>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public Task<Game> GetGame(Guid gameId, Guid userId)
        {
            return dbContext.Games
                .Where(AccessFilter(userId))
                .Where(g => g.GameId == gameId)
                .ProjectTo<Game>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GameTag>> GetTags()
        {
            return await dbContext.Tags
                .ProjectTo<GameTag>()
                .ToArrayAsync();
        }
    }
}