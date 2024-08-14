using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Games.Shared;

/// <inheritdoc />
internal class UserRepository : IUserRepository
{
    private readonly DmDbContext dbContext;

    /// <inheritdoc />
    public UserRepository(
        DmDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<(bool exists, Guid userId)> FindUserId(string login)
    {
        var foundUserId = await dbContext.Users
            .Where(u => !u.IsRemoved && u.Activated && u.Login.ToLower() == login.ToLower())
            .Select(u => u.UserId)
            .FirstOrDefaultAsync();
        return (foundUserId != default, foundUserId);
    }

    /// <inheritdoc />
    public Task<bool> UserExists(string login, CancellationToken cancellationToken) =>
        dbContext.Users.AnyAsync(u =>
            u.Login.ToLower() == login.ToLower() &&
            !u.IsRemoved &&
            u.Activated, cancellationToken);
}