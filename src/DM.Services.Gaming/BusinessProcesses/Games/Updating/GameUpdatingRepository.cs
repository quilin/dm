using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;
using Game = DM.Services.DataAccess.BusinessObjects.Games.Game;

namespace DM.Services.Gaming.BusinessProcesses.Games.Updating;

/// <inheritdoc />
internal class GameUpdatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IGameUpdatingRepository
{
    /// <inheritdoc />
    public async Task<GameExtended> Update(IUpdateBuilder<Game> updateGame, CancellationToken cancellationToken)
    {
        var gameId = updateGame.AttachTo(dbContext);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await dbContext.Games
            .Where(g => g.GameId == gameId)
            .ProjectTo<GameExtended>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}