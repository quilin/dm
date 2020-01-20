using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Reading
{
    /// <inheritdoc />
    public class RoomClaimsReadingService : IRoomClaimsReadingService
    {
        private readonly IRoomClaimsReadingRepository repository;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public RoomClaimsReadingService(
            IRoomClaimsReadingRepository repository,
            IIdentityProvider identityProvider)
        {
            this.repository = repository;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public Task<IEnumerable<RoomClaim>> GetGameClaims(Guid gameId) =>
            repository.GetGameClaims(gameId, identity.User.UserId);

        /// <inheritdoc />
        public Task<IEnumerable<RoomClaim>> GetRoomClaims(Guid roomId) =>
            repository.GetRoomClaims(roomId, identity.User.UserId);

        /// <inheritdoc />
        public async Task<RoomClaim> GetClaim(Guid claimId)
        {
            var claim = await repository.GetClaim(claimId, identity.User.UserId);
            if (claim == null)
            {
                throw new HttpException(HttpStatusCode.Gone, "Claim not found");
            }

            return claim;
        }
    }
}