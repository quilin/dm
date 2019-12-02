using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.Gaming.BusinessProcesses.Shared;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Reading
{
    /// <inheritdoc />
    public class RoomClaimsReadingRepository : IRoomClaimsReadingRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public RoomClaimsReadingRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        
        /// <inheritdoc />
        public async Task<IEnumerable<RoomClaim>> GetAll(Guid roomId, Guid userId)
        {
            return await dbContext.Rooms
                .Where(r => r.RoomId == roomId)
                .Where(AccessibilityFilters.RoomAvailable(userId))
                .Select(r => r.ParticipantLinks)
                .ProjectTo<RoomClaim>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        /// <inheritdoc />
        public async Task<RoomClaim> Get(Guid claimId, Guid userId)
        {
            return await dbContext.ParticipantRoomLinks
                .Where(l => l.ParticipantRoomLinkId == claimId)
                .Where(l => AccessibilityFilters.RoomAvailable(userId).Compile().Invoke(l.Room))
                .ProjectTo<RoomClaim>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}