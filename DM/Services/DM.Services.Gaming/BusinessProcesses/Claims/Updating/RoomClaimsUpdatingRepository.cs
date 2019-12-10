using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;
using RoomClaim = DM.Services.DataAccess.BusinessObjects.Games.Links.RoomClaim;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Updating
{
    /// <inheritdoc />
    public class RoomClaimsUpdatingRepository : IRoomClaimsUpdatingRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public RoomClaimsUpdatingRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<Dto.Output.RoomClaim> Update(IUpdateBuilder<RoomClaim> updateClaim)
        {
            var linkId = updateClaim.AttachTo(dbContext);
            await dbContext.SaveChangesAsync();
            return await dbContext.RoomClaims
                .Where(l => l.RoomClaimId == linkId)
                .ProjectTo<Dto.Output.RoomClaim>(mapper.ConfigurationProvider)
                .FirstAsync();
        }
    }
}