using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.BusinessProcesses.Account.Registration;

/// <inheritdoc />
internal class RegistrationRepository(
    DmDbContext dbContext) : IRegistrationRepository
{
    /// <inheritdoc />
    public async Task<bool> EmailFree(string email, CancellationToken cancellationToken) =>
        !await dbContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);

    /// <inheritdoc />
    public async Task<bool> LoginFree(string login, CancellationToken cancellationToken) =>
        !await dbContext.Users.AnyAsync(u => u.Login.ToLower() == login.ToLower(), cancellationToken);

    /// <inheritdoc />
    public Task AddUser(User user, Token token, CancellationToken cancellationToken)
    {
        dbContext.Users.Add(user);
        dbContext.Tokens.Add(token);
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}