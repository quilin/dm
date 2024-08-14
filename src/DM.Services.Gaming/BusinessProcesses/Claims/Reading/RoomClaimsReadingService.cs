using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Reading;

/// <inheritdoc />
internal class RoomClaimsReadingService : IRoomClaimsReadingService
{
    private readonly IRoomClaimsReadingRepository repository;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public RoomClaimsReadingService(
        IRoomClaimsReadingRepository repository,
        IIdentityProvider identityProvider)
    {
        this.repository = repository;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public Task<IEnumerable<RoomClaim>> GetGameClaims(Guid gameId) =>
        repository.GetGameClaims(gameId, identityProvider.Current.User.UserId);

    /// <inheritdoc />
    public Task<IEnumerable<RoomClaim>> GetRoomClaims(Guid roomId) =>
        repository.GetRoomClaims(roomId, identityProvider.Current.User.UserId);

    /// <inheritdoc />
    public async Task<RoomClaim> GetClaim(Guid claimId)
    {
        var claim = await repository.GetClaim(claimId, identityProvider.Current.User.UserId);
        if (claim == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Claim not found");
        }

        return claim;
    }
}