using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.BusinessProcesses.Account.Verification;

/// <inheritdoc />
internal class TokenVerificationRepository(
    DmDbContext dbContext,
    IMapper mapper) : ITokenVerificationRepository
{
    /// <inheritdoc />
    public Task<GeneralUser> GetTokenOwner(Guid tokenId, CancellationToken cancellationToken) => dbContext.Tokens
        .Where(t => t.TokenId == tokenId && !t.IsRemoved)
        .Select(t => t.User)
        .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(cancellationToken);
}