using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Games.Shared
{
    /// <inheritdoc />
    public class UserRepository : IUserRepository
    {
        private readonly DmDbContext dbContext;

        /// <inheritdoc />
        public UserRepository(
            DmDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc />
        public Task<bool> UserExists(string login, CancellationToken cancellationToken) =>
            dbContext.Users.AnyAsync(u =>
                u.Login.ToLower() == login.ToLower() &&
                !u.IsRemoved &&
                u.Activated, cancellationToken);
    }
}