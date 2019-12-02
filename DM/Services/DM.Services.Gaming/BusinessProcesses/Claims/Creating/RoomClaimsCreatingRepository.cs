using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Creating
{
    /// <inheritdoc />
    public class RoomClaimsCreatingRepository : IRoomClaimsCreatingRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public RoomClaimsCreatingRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<RoomClaim> Create(ParticipantRoomLink link)
        {
            dbContext.ParticipantRoomLinks.Add(link);
            await dbContext.SaveChangesAsync();
            return await dbContext.ParticipantRoomLinks
                .Where(l => l.ParticipantRoomLinkId == link.ParticipantRoomLinkId)
                .ProjectTo<RoomClaim>(mapper.ConfigurationProvider)
                .FirstAsync();
        }

        /// <inheritdoc />
        public async Task<Guid?> FindReaderId(Guid gameId, string readerLogin)
        {
            var readerWrapper = await dbContext.Readers
                .Where(r => r.User.Login == readerLogin && r.GameId == gameId)
                .Select(r => new {r.ReaderId})
                .FirstOrDefaultAsync();
            return readerWrapper?.ReaderId;
        }

        /// <inheritdoc />
        public async Task<Guid?> FindCharacterGameId(Guid characterId)
        {
            var gameWrapper = await dbContext.Characters
                .Where(c => c.CharacterId == characterId && !c.IsRemoved)
                .Select(c => new {c.GameId})
                .FirstOrDefaultAsync();
            return gameWrapper?.GameId;
        }
    }
}