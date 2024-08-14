using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.BusinessProcesses.Moderation;

/// <inheritdoc />
internal class ModeratorRepository : IModeratorRepository
{
    private readonly DmDbContext dmDbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public ModeratorRepository(
        DmDbContext dmDbContext,
        IMapper mapper)
    {
        this.dmDbContext = dmDbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GeneralUser>> Get(Guid forumId)
    {
        return await dmDbContext.ForumModerators
            .TagWith("DM.Forum.ModeratorsList")
            .Where(m => m.ForumId == forumId)
            .Select(m => m.User)
            .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
            .ToArrayAsync();
    }
}