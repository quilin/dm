using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Deleting;

/// <inheritdoc />
internal class RoomClaimsDeletingRepository(
    DmDbContext dbContext) : IRoomClaimsDeletingRepository
{
    /// <inheritdoc />
    public Task Delete(IUpdateBuilder<RoomClaim> deleteLink, CancellationToken cancellationToken)
    {
        deleteLink.AttachTo(dbContext);
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}