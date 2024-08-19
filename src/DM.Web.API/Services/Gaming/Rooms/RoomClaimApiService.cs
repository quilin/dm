using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Gaming.BusinessProcesses.Claims.Creating;
using DM.Services.Gaming.BusinessProcesses.Claims.Deleting;
using DM.Services.Gaming.BusinessProcesses.Claims.Updating;
using DM.Services.Gaming.Dto.Input;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;

namespace DM.Web.API.Services.Gaming.Rooms;

/// <inheritdoc />
internal class RoomClaimApiService(
    IRoomClaimsCreatingService creatingService,
    IRoomClaimsUpdatingService updatingService,
    IRoomClaimsDeletingService deletingService,
    IMapper mapper) : IRoomClaimApiService
{
    /// <inheritdoc />
    public async Task<Envelope<RoomClaim>> Create(
        Guid roomId, RoomClaim claim, CancellationToken cancellationToken)
    {
        var createRoomClaim = mapper.Map<CreateRoomClaim>(claim);
        createRoomClaim.RoomId = roomId;
        var createdRoomClaim = await creatingService.Create(createRoomClaim, cancellationToken);
        return new Envelope<RoomClaim>(mapper.Map<RoomClaim>(createdRoomClaim));
    }

    /// <inheritdoc />
    public async Task<Envelope<RoomClaim>> Update(
        Guid claimId, RoomClaim claim, CancellationToken cancellationToken)
    {
        var updateRoomClaim = mapper.Map<UpdateRoomClaim>(claim);
        updateRoomClaim.ClaimId = claimId;
        var updatedRoomClaim = await updatingService.Update(updateRoomClaim, cancellationToken);
        return new Envelope<RoomClaim>(mapper.Map<RoomClaim>(updatedRoomClaim));
    }

    /// <inheritdoc />
    public Task Delete(Guid claimId, CancellationToken cancellationToken) =>
        deletingService.Delete(claimId, cancellationToken);
}