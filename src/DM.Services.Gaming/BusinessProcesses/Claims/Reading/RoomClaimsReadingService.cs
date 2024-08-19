using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Reading;

/// <inheritdoc />
internal class RoomClaimsReadingService(
    IRoomClaimsReadingRepository repository,
    IIdentityProvider identityProvider) : IRoomClaimsReadingService
{
    /// <inheritdoc />
    public Task<IEnumerable<RoomClaim>> GetGameClaims(Guid gameId, CancellationToken cancellationToken) =>
        repository.GetGameClaims(gameId, identityProvider.Current.User.UserId, cancellationToken);

    /// <inheritdoc />
    public Task<IEnumerable<RoomClaim>> GetRoomClaims(Guid roomId, CancellationToken cancellationToken) =>
        repository.GetRoomClaims(roomId, identityProvider.Current.User.UserId, cancellationToken);

    /// <inheritdoc />
    public async Task<RoomClaim> GetClaim(Guid claimId, CancellationToken cancellationToken)
    {
        var claim = await repository.GetClaim(claimId, identityProvider.Current.User.UserId, cancellationToken);
        if (claim == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Claim not found");
        }

        return claim;
    }
}