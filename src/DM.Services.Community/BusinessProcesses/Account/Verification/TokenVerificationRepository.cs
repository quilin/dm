using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.BusinessProcesses.Account.Verification;

/// <inheritdoc />
internal class TokenVerificationRepository : ITokenVerificationRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public TokenVerificationRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public Task<GeneralUser> GetTokenOwner(Guid tokenId) => dbContext.Tokens
        .Where(t => t.TokenId == tokenId && !t.IsRemoved)
        .Select(t => t.User)
        .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();
}