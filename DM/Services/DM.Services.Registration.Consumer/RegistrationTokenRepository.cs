using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Registration.Consumer
{
    /// <inheritdoc />
    public class RegistrationTokenRepository : IRegistrationTokenRepository
    {
        private readonly DmDbContext dbContext;

        /// <inheritdoc />
        public RegistrationTokenRepository(
            DmDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc />
        public Task Add(Token token)
        {
            dbContext.Tokens.Add(token);
            return dbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public Task<string> GetRegisteredUserEmail(Guid userId)
        {
            return dbContext.Users
                .Where(u => u.UserId == userId)
                .Select(u => u.Email)
                .FirstAsync();
        }
    }
}