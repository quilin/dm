using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.BusinessProcesses.Account.Activation;

/// <inheritdoc />
internal class ActivationRepository : IActivationRepository
{
    private readonly DmDbContext dbContext;

    /// <inheritdoc />
    public ActivationRepository(
        DmDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
        
    /// <inheritdoc />
    public async Task<Guid?> FindUserToActivate(Guid tokenId, DateTimeOffset createdSince)
    {
        return (await dbContext.Tokens
            .Where(t => t.TokenId == tokenId && t.CreateDate > createdSince)
            .Select(t => new {t.UserId})
            .FirstOrDefaultAsync())?.UserId;
    }

    /// <inheritdoc />
    public Task ActivateUser(IUpdateBuilder<User> updateUser, IUpdateBuilder<Token> updateToken)
    {
        updateUser.AttachTo(dbContext);
        updateToken.AttachTo(dbContext);
        return dbContext.SaveChangesAsync();
    }
}