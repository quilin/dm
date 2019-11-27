using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;

namespace DM.Web.API.Services.Gaming
{
    /// <inheritdoc />
    public class RoomApiService : IRoomApiService
    {
        /// <inheritdoc />
        public Task<ListEnvelope<Room>> GetAll(Guid gameId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<Envelope<Room>> Get(Guid roomId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<Envelope<Room>> Create(Guid gameId, Room room)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<Envelope<Room>> Update(Guid roomId, Room room)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task Delete(Guid roomId)
        {
            throw new NotImplementedException();
        }
    }
}