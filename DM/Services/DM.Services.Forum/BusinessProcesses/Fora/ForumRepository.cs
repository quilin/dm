using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Caching;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.BusinessProcesses.Fora;

/// <inheritdoc />
internal class ForumRepository : IForumRepository
{
    private readonly DmDbContext dmDbContext;
    private readonly ICache cache;
    private readonly IMapper mapper;

    public ForumRepository(
        DmDbContext dmDbContext,
        ICache cache,
        IMapper mapper)
    {
        this.dmDbContext = dmDbContext;
        this.cache = cache;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Dto.Output.Forum>> SelectFora(ForumAccessPolicy? accessPolicy)
    {
        var forums = await cache.GetOrCreate("Fora", () => dmDbContext.Fora
            .OrderBy(f => f.Order)
            .ProjectTo<Dto.Output.Forum>(mapper.ConfigurationProvider)
            .ToArrayAsync());

        if (accessPolicy.HasValue)
        {
            forums = forums
                .Where(f => (f.ViewPolicy & accessPolicy) != ForumAccessPolicy.NoOne)
                .ToArray();
        }

        return forums.Select(mapper.Map<Dto.Output.Forum>);
    }
}