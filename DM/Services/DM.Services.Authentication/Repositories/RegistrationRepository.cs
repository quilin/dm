using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Authentication.Repositories
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
        public Task<bool> EmailFree(string email, CancellationToken cancellationToken) =>
            dbContext.Users.AllAsync(u => u.Email.ToLower() != email.ToLower(), cancellationToken);

        /// <inheritdoc />
        public Task<bool> LoginFree(string login, CancellationToken cancellationToken) =>
            dbContext.Users.AllAsync(u => u.Login.ToLower() != login.ToLower(), cancellationToken);

        /// <inheritdoc />
        public Task AddUser(User user, Token token)
        {
            dbContext.Users.Add(user);
            dbContext.Tokens.Add(token);
            return dbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public Task<Guid> FindUserToActivate(Guid tokenId, DateTime createdSince)
        {
            return dbContext.Tokens
                .Where(t => t.TokenId == tokenId && t.CreateDate > createdSince)
                .Select(t => t.UserId)
                .FirstOrDefaultAsync();
        }
        /// <inheritdoc />
        public Task ActivateUser(UpdateBuilder<User> updateUser, UpdateBuilder<Token> updateToken)
        {
            updateUser.Update(dbContext);
            updateToken.Update(dbContext);
            return dbContext.SaveChangesAsync();
        }
    }
}