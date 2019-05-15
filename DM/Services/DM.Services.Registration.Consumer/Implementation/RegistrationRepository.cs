using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Registration.Consumer.Implementation
{
    /// <inheritdoc />
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly DmDbContext dbContext;

        /// <inheritdoc />
        public RegistrationRepository(
            DmDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        /// <inheritdoc />
        public Task<RegistrationMailViewModel> Get(Guid userId)
        {
            return dbContext.Users
                .Where(u => u.UserId == userId)
                .Select(u => new RegistrationMailViewModel
                {
                    Email = u.Email,
                    Login = u.Login,
                    Token = u.Tokens
                        .Where(t => !t.IsRemoved)
                        .Select(t => t.TokenId)
                        .First()
                })
                .FirstAsync();
        }
    }
}