using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;
using DbRoomClaim = DM.Services.DataAccess.BusinessObjects.Games.Links.RoomClaim;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Updating;

/// <inheritdoc />
internal class RoomClaimsUpdatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IRoomClaimsUpdatingRepository
{
    /// <inheritdoc />
    public async Task<RoomClaim> Update(IUpdateBuilder<DbRoomClaim> updateClaim, CancellationToken cancellationToken)
    {
        var linkId = updateClaim.AttachTo(dbContext);
        await dbContext.SaveChangesAsync(cancellationToken);
        return await dbContext.RoomClaims
            .Where(l => l.RoomClaimId == linkId)
            .ProjectTo<RoomClaim>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}