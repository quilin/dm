using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
internal class GameCreatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IGameCreatingRepository
{
    /// <inheritdoc />
    public async Task<GameExtended> Create(DbGame game, DbRoom room,
        IEnumerable<DbTag> tags, CancellationToken cancellationToken)
    {
        await dbContext.Games.AddAsync(game, cancellationToken);
        await dbContext.Rooms.AddAsync(room, cancellationToken);
        await dbContext.GameTags.AddRangeAsync(tags, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return await dbContext.Games
            .Where(g => g.GameId == game.GameId)
            .ProjectTo<GameExtended>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}