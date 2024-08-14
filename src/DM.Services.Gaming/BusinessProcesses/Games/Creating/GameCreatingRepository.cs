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

namespace DM.Services.Gaming.BusinessProcesses.Games.Creating;

/// <inheritdoc />
internal class GameCreatingRepository : IGameCreatingRepository
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
        await dbContext.Games.AddAsync(game);
        await dbContext.Rooms.AddAsync(room);
        await dbContext.GameTags.AddRangeAsync(tags);
        await dbContext.SaveChangesAsync();
        return await dbContext.Games
            .Where(g => g.GameId == game.GameId)
            .ProjectTo<GameExtended>(mapper.ConfigurationProvider)
            .FirstAsync();
    }
}