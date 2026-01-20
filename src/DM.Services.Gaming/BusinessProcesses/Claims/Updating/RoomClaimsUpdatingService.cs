using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Claims.Reading;
using DM.Services.Gaming.BusinessProcesses.Rooms.Updating;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using FluentValidation;
using DbRoomClaim = DM.Services.DataAccess.BusinessObjects.Games.Links.RoomClaim;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Updating;

/// <inheritdoc />
internal class RoomClaimsUpdatingService(
    IValidator<UpdateRoomClaim> validator,
    IRoomClaimsUpdatingRepository repository,
    IRoomClaimsReadingRepository readingRepository,
    IRoomUpdatingRepository roomUpdatingRepository,
    IIntentionManager intentionManager,
    IUpdateBuilderFactory updateBuilderFactory,
    IIdentityProvider identityProvider) : IRoomClaimsUpdatingService
{
    /// <inheritdoc />
    public async Task<RoomClaim> Update(UpdateRoomClaim updateRoomClaim, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(updateRoomClaim, cancellationToken);
        var currentUserId = identityProvider.Current.User.UserId;
        var oldClaim = await readingRepository.GetClaim(updateRoomClaim.ClaimId, currentUserId, cancellationToken);
        var room = await roomUpdatingRepository.GetRoom(oldClaim.RoomId, currentUserId, cancellationToken);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, room.Game);

        if (oldClaim.User != null && updateRoomClaim.Policy == RoomAccessPolicy.Full)
        {
            throw new HttpBadRequestException(new Dictionary<string, string>
            {
                [nameof(RoomClaim.Policy)] = ValidationError.Invalid
            });
        }

        var updateBuilder = updateBuilderFactory.Create<DbRoomClaim>(updateRoomClaim.ClaimId)
            .Field(c => c.Policy, updateRoomClaim.Policy);
        return await repository.Update(updateBuilder, cancellationToken);
    }
}