using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Community.BusinessProcesses.Account.PasswordReset;

/// <inheritdoc />
internal class PasswordResetRepository : IPasswordResetRepository
{
    private readonly DmDbContext dbContext;

    /// <inheritdoc />
    public PasswordResetRepository(
        DmDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task CreateToken(Token token)
    {
        dbContext.Tokens.Add(token);
        return dbContext.SaveChangesAsync();
    }
}