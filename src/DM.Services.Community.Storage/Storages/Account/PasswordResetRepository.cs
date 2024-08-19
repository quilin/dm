using DM.Services.Community.BusinessProcesses.Account.PasswordReset;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Community.Storage.Storages.Account;

/// <inheritdoc />
internal class PasswordResetRepository(
    DmDbContext dbContext) : IPasswordResetRepository
{
    /// <inheritdoc />
    public Task CreateToken(Token token, CancellationToken cancellationToken)
    {
        dbContext.Tokens.Add(token);
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}