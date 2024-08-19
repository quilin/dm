using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Authentication.Dto;
using DM.Services.Community.BusinessProcesses.Account.PasswordChange;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.Storage.Storages.Account;

/// <inheritdoc />
internal class PasswordChangeRepository(
    DmDbContext dbContext,
    IMapper mapper) : IPasswordChangeRepository
{
    /// <inheritdoc />
    public Task<AuthenticatedUser?> FindUser(string login, CancellationToken cancellationToken) =>
        dbContext.Users
            .Where(u => u.Login.ToLower() == login.ToLower())
            .ProjectTo<AuthenticatedUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public Task<AuthenticatedUser?> FindUser(Guid tokenId, CancellationToken cancellationToken) =>
        dbContext.Tokens
            .Where(u => u.TokenId == tokenId)
            .Select(u => u.User)
            .ProjectTo<AuthenticatedUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public Task<bool> TokenValid(Guid tokenId, DateTimeOffset createdSince, CancellationToken cancellationToken) =>
        dbContext.Tokens
            .Where(t => t.TokenId == tokenId && t.Type == TokenType.PasswordChange && t.CreateDate > createdSince)
            .AnyAsync(cancellationToken);

    /// <inheritdoc />
    public Task UpdatePassword(IUpdateBuilder<User> userUpdate, IUpdateBuilder<Token> tokenUpdate,
        CancellationToken cancellationToken)
    {
        userUpdate.AttachTo(dbContext);
        tokenUpdate?.AttachTo(dbContext);
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}