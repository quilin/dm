using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Deleting;

/// <inheritdoc />
internal class RoomClaimsDeletingRepository : IRoomClaimsDeletingRepository
{
    private readonly DmDbContext dbContext;

    /// <inheritdoc />
    public RoomClaimsDeletingRepository(
        DmDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
        
    /// <inheritdoc />
    public Task Delete(IUpdateBuilder<RoomClaim> deleteLink)
    {
        deleteLink.AttachTo(dbContext);
        return dbContext.SaveChangesAsync();
    }
}