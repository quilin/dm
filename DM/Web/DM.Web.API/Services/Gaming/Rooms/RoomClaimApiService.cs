using System;
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
internal class RoomClaimApiService : IRoomClaimApiService
{
    private readonly IRoomClaimsCreatingService creatingService;
    private readonly IRoomClaimsUpdatingService updatingService;
    private readonly IRoomClaimsDeletingService deletingService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public RoomClaimApiService(
        IRoomClaimsCreatingService creatingService,
        IRoomClaimsUpdatingService updatingService,
        IRoomClaimsDeletingService deletingService,
        IMapper mapper)
    {
        this.creatingService = creatingService;
        this.updatingService = updatingService;
        this.deletingService = deletingService;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Envelope<RoomClaim>> Create(Guid roomId, RoomClaim claim)
    {
        var createRoomClaim = mapper.Map<CreateRoomClaim>(claim);
        createRoomClaim.RoomId = roomId;
        var createdRoomClaim = await creatingService.Create(createRoomClaim);
        return new Envelope<RoomClaim>(mapper.Map<RoomClaim>(createdRoomClaim));
    }

    /// <inheritdoc />
    public async Task<Envelope<RoomClaim>> Update(Guid claimId, RoomClaim claim)
    {
        var updateRoomClaim = mapper.Map<UpdateRoomClaim>(claim);
        updateRoomClaim.ClaimId = claimId;
        var updatedRoomClaim = await updatingService.Update(updateRoomClaim);
        return new Envelope<RoomClaim>(mapper.Map<RoomClaim>(updatedRoomClaim));
    }

    /// <inheritdoc />
    public Task Delete(Guid claimId) => deletingService.Delete(claimId);
}