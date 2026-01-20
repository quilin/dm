using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Readers.Reading;

/// <inheritdoc />
internal class ReadersReadingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IReadersReadingRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<GeneralUser>> Get(Guid gameId, Guid userId, CancellationToken cancellationToken) =>
        await dbContext.Readers
            .Where(r => r.GameId == gameId)
            .Select(g => g.User)
            .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);
}