using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Creating;

/// <inheritdoc />
internal class BlacklistCreatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IBlacklistCreatingRepository
{
    /// <inheritdoc />
    public async Task<GeneralUser> Create(BlackListLink link, CancellationToken cancellationToken)
    {
        dbContext.BlackListLinks.Add(link);
        await dbContext.SaveChangesAsync(cancellationToken);
        return await dbContext.BlackListLinks
            .Where(l => l.BlackListLinkId == link.BlackListLinkId)
            .Select(l => l.User)
            .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}