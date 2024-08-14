using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Creating;

/// <inheritdoc />
internal class BlacklistCreatingRepository : IBlacklistCreatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public BlacklistCreatingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
        
    /// <inheritdoc />
    public async Task<GeneralUser> Create(BlackListLink link)
    {
        dbContext.BlackListLinks.Add(link);
        await dbContext.SaveChangesAsync();
        return await dbContext.BlackListLinks
            .Where(l => l.BlackListLinkId == link.BlackListLinkId)
            .Select(l => l.User)
            .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
            .FirstAsync();
    }
}