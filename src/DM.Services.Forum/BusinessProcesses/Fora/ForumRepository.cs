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
internal class ForumRepository(
    DmDbContext dmDbContext,
    ICache cache,
    IMapper mapper)
    : IForumRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<Dto.Output.Forum>> SelectFora(ForumAccessPolicy? accessPolicy)
    {
        var forums = await cache.GetOrCreate<IEnumerable<Dto.Output.Forum>>("Fora", async () => await dmDbContext.Fora
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