using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Community.BusinessProcesses.Account.PasswordReset;

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