using DM.Services.Community.BusinessProcesses.Account.Activation;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.Storage.Storages.Account;

/// <inheritdoc />
internal class ActivationRepository(
    DmDbContext dbContext) : IActivationRepository
{
    /// <inheritdoc />
    public async Task<Guid?> FindUserToActivate(Guid tokenId, DateTimeOffset createdSince,
        CancellationToken cancellationToken) =>
        (await dbContext.Tokens
            .Where(t => t.TokenId == tokenId && t.CreateDate > createdSince)
            .Select(t => new {t.UserId})
            .FirstOrDefaultAsync(cancellationToken))?.UserId;

    /// <inheritdoc />
    public Task ActivateUser(IUpdateBuilder<User> updateUser, IUpdateBuilder<Token> updateToken,
        CancellationToken cancellationToken)
    {
        updateUser.AttachTo(dbContext);
        updateToken.AttachTo(dbContext);
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}