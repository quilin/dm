using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <inheritdoc />
        public Task<int> Count(GameStatus? status)
        {
            return dbContext.Games
                .Where(g => !g.IsRemoved)
                .Where(g => !status.HasValue || g.Status == status.Value)
                .CountAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Game>> GetGames(PagingData pagingData, GameStatus? status)
        {
            return await dbContext.Games
                .Where(g => !g.IsRemoved)
                .Where(g => !status.HasValue || g.Status == status.Value)
                .OrderBy(g => g.ReleaseDate ?? g.CreateDate)
                .Skip(pagingData.Skip)
                .Take(pagingData.Take)
                .ProjectTo<Game>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        /// <inheritdoc />
        public Task<GameExtended> GetGame(Guid gameId)
        {
            return dbContext.Games
                .Where(g => !g.IsRemoved && g.GameId == gameId)
                .ProjectTo<GameExtended>(mapper.ConfigurationProvider)
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