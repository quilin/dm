using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.BusinessProcesses.Activation
{
    /// <inheritdoc />
    public class ActivationRepository : IActivationRepository
    {
        private readonly DmDbContext dbContext;

        /// <inheritdoc />
        public ActivationRepository(
            DmDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        /// <inheritdoc />
        public Task<Guid> FindUserToActivate(Guid tokenId, DateTimeOffset createdSince)
        {
            return dbContext.Tokens
                .Where(t => t.TokenId == tokenId && t.CreateDate > createdSince)
                .Select(t => t.UserId)
                .FirstOrDefaultAsync();
        }
        /// <inheritdoc />
        public Task ActivateUser(IUpdateBuilder<User> updateUser, IUpdateBuilder<Token> updateToken)
        {
            updateUser.AttachTo(dbContext);
            updateToken.AttachTo(dbContext);
            return dbContext.SaveChangesAsync();
        }
    }
}