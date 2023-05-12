using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DM.Services.Forum.BusinessProcesses.Fora;

/// <inheritdoc />
internal class ForumRepository : IForumRepository
{
    private readonly DmDbContext dmDbContext;
    private readonly IMemoryCache cache;
    private readonly IMapper mapper;

    public ForumRepository(
        DmDbContext dmDbContext,
        IMemoryCache cache,
        IMapper mapper)
    {
        this.dmDbContext = dmDbContext;
        this.cache = cache;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Dto.Output.Forum>> SelectFora(ForumAccessPolicy? accessPolicy)
    {
        var forums = await cache.GetOrCreateAsync<IEnumerable<Dto.Output.Forum>>("Fora", async _ => await dmDbContext.Fora
            .TagWith("DM.Forum.ForaList")
            .OrderBy(f => f.Order)
            .ProjectTo<Dto.Output.Forum>(mapper.ConfigurationProvider)
            .ToArrayAsync());

        if (accessPolicy.HasValue)
        {
            forums = forums.Where(f => (f.ViewPolicy & accessPolicy) != ForumAccessPolicy.NoOne);
        }

        return forums.Select(mapper.Map<Dto.Output.Forum>).ToArray();
    }
}