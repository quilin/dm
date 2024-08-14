using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using RoomClaim = DM.Services.DataAccess.BusinessObjects.Games.Links.RoomClaim;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Creating;

/// <inheritdoc />
internal class RoomClaimsCreatingRepository : IRoomClaimsCreatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public RoomClaimsCreatingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Dto.Output.RoomClaim> Create(RoomClaim claim)
    {
        dbContext.RoomClaims.Add(claim);
        await dbContext.SaveChangesAsync();
        return await dbContext.RoomClaims
            .Where(l => l.RoomClaimId == claim.RoomClaimId)
            .ProjectTo<Dto.Output.RoomClaim>(mapper.ConfigurationProvider)
            .FirstAsync();
    }

    /// <inheritdoc />
    public async Task<Guid?> FindReaderId(Guid gameId, string readerLogin)
    {
        var readerWrapper = await dbContext.Readers
            .Where(r => r.User.Login == readerLogin && r.GameId == gameId)
            .Select(r => new {r.ReaderId})
            .FirstOrDefaultAsync();
        return readerWrapper?.ReaderId;
    }

    /// <inheritdoc />
    public async Task<Guid?> FindCharacterGameId(Guid characterId)
    {
        var gameWrapper = await dbContext.Characters
            .Where(c => c.CharacterId == characterId && !c.IsRemoved)
            .Select(c => new {c.GameId})
            .FirstOrDefaultAsync();
        return gameWrapper?.GameId;
    }
}