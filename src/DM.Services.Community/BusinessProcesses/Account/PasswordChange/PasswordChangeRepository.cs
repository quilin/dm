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

namespace DM.Services.Community.BusinessProcesses.Account.PasswordChange;

/// <inheritdoc />
internal class PasswordChangeRepository : IPasswordChangeRepository
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
    public Task<AuthenticatedUser> FindUser(Guid tokenId) => dbContext.Tokens
        .Where(u => u.TokenId == tokenId)
        .Select(u => u.User)
        .ProjectTo<AuthenticatedUser>(mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();

    /// <inheritdoc />
    public Task<bool> TokenValid(Guid tokenId, DateTimeOffset createdSince) => dbContext.Tokens
        .AnyAsync(t => t.TokenId == tokenId &&
                       t.Type == TokenType.PasswordChange && t.CreateDate > createdSince);

    /// <inheritdoc />
    public Task UpdatePassword(IUpdateBuilder<User> userUpdate, IUpdateBuilder<Token> tokenUpdate)
    {
        userUpdate.AttachTo(dbContext);
        tokenUpdate?.AttachTo(dbContext);
        return dbContext.SaveChangesAsync();
    }
}