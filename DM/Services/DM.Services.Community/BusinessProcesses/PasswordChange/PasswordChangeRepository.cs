using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Authentication.Dto;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.BusinessProcesses.PasswordChange
{
    /// <inheritdoc />
    public class PasswordChangeRepository : IPasswordChangeRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public PasswordChangeRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public Task<AuthenticatedUser> FindUser(string login) => dbContext.Users
            .Where(u => u.Login.ToLower() == login.ToLower())
            .ProjectTo<AuthenticatedUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        /// <inheritdoc />
        public Task<bool> TokenValid(Guid tokenId, Guid userId, DateTimeOffset createdSince) => dbContext.Tokens
            .AnyAsync(t => t.TokenId == tokenId && t.UserId == userId &&
                t.Type == TokenType.PasswordRestoration && t.CreateDate > createdSince);

        /// <inheritdoc />
        public Task UpdatePassword(IUpdateBuilder<User> userUpdate, IUpdateBuilder<Token> tokenUpdate)
        {
            userUpdate.AttachTo(dbContext);
            tokenUpdate?.AttachTo(dbContext);
            return dbContext.SaveChangesAsync();
        }
    }
}