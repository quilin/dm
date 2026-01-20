using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using RoomClaim = DM.Services.DataAccess.BusinessObjects.Games.Links.RoomClaim;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Creating;

/// <inheritdoc />
internal class RoomClaimsCreatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IRoomClaimsCreatingRepository
{
    /// <inheritdoc />
    public async Task<Dto.Output.RoomClaim> Create(RoomClaim claim, CancellationToken cancellationToken)
    {
        dbContext.RoomClaims.Add(claim);
        await dbContext.SaveChangesAsync(cancellationToken);
        return await dbContext.RoomClaims
            .Where(l => l.RoomClaimId == claim.RoomClaimId)
            .ProjectTo<Dto.Output.RoomClaim>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Guid?> FindReaderId(Guid gameId, string readerLogin, CancellationToken cancellationToken)
    {
        var readerWrapper = await dbContext.Readers
            .Where(r => r.User.Login == readerLogin && r.GameId == gameId)
            .Select(r => new {r.ReaderId})
            .FirstOrDefaultAsync(cancellationToken);
        return readerWrapper?.ReaderId;
    }

    /// <inheritdoc />
    public async Task<Guid?> FindCharacterGameId(Guid characterId, CancellationToken cancellationToken)
    {
        var gameWrapper = await dbContext.Characters
            .Where(c => c.CharacterId == characterId && !c.IsRemoved)
            .Select(c => new {c.GameId})
            .FirstOrDefaultAsync(cancellationToken);
        return gameWrapper?.GameId;
    }
}