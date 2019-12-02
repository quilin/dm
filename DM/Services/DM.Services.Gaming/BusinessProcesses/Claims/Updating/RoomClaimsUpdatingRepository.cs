using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;

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
        public async Task<RoomClaim> Update(IUpdateBuilder<ParticipantRoomLink> updateClaim)
        {
            var linkId = updateClaim.AttachTo(dbContext);
            await dbContext.SaveChangesAsync();
            return await dbContext.ParticipantRoomLinks
                .Where(l => l.ParticipantRoomLinkId == linkId)
                .ProjectTo<RoomClaim>(mapper.ConfigurationProvider)
                .FirstAsync();
        }
    }
}