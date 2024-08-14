using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Reading;

/// <inheritdoc />
internal class BlacklistReadingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IBlacklistReadingRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<GeneralUser>> Get(Guid gameId) => await dbContext.BlackListLinks
        .Where(l => l.GameId == gameId)
        .Select(l => l.User)
        .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
        .ToArrayAsync();
}