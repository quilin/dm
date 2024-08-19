using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Community.BusinessProcesses.Account.Verification;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.Storage.Storages.Account;

/// <inheritdoc />
internal class TokenVerificationRepository(
    DmDbContext dbContext,
    IMapper mapper) : ITokenVerificationRepository
{
    /// <inheritdoc />
    public Task<GeneralUser?> GetTokenOwner(Guid tokenId, CancellationToken cancellationToken) =>
        dbContext.Tokens
            .Where(t => t.TokenId == tokenId && !t.IsRemoved)
            .Select(t => t.User)
            .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
}