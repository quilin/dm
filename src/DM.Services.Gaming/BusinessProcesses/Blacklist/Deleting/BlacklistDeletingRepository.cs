using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Deleting;

/// <inheritdoc />
internal class BlacklistDeletingRepository : IBlacklistDeletingRepository
{
    private readonly DmDbContext dbContext;

    /// <inheritdoc />
    public BlacklistDeletingRepository(
        DmDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
        
    /// <inheritdoc />
    public Task Delete(IUpdateBuilder<BlackListLink> updateBuilder)
    {
        updateBuilder.AttachTo(dbContext);
        return dbContext.SaveChangesAsync();
    }
}