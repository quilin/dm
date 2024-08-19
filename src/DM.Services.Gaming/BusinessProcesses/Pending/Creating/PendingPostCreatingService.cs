using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Shared;
using DM.Services.Gaming.BusinessProcesses.Rooms.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;

namespace DM.Services.Gaming.BusinessProcesses.Pending.Creating;

/// <inheritdoc />
internal class PendingPostCreatingService(
    IValidator<CreatePendingPost> validator,
    IRoomReadingService roomReadingService,
    IIntentionManager intentionManager,
    IPendingPostFactory factory,
    IUserRepository userRepository,
    IPendingPostCreatingRepository repository,
    IInvokedEventProducer producer,
    IIdentityProvider identityProvider) : IPendingPostCreatingService
{
    /// <inheritdoc />
    public async Task<PendingPost> Create(CreatePendingPost createPendingPost, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createPendingPost, cancellationToken);
        var room = await roomReadingService.Get(createPendingPost.RoomId, cancellationToken);
        intentionManager.ThrowIfForbidden(RoomIntention.CreatePendingPost, room);

        var (_, pendingUserId) = await userRepository.FindUserId(createPendingPost.PendingUserLogin, cancellationToken);
        var currentUserId = identityProvider.Current.User.UserId;
        if (room.Pendings.Any(p =>
                p.AwaitingUser.UserId == currentUserId &&
                p.PendingUser.UserId == pendingUserId))
        {
            throw new HttpException(HttpStatusCode.Conflict,
                $"There's already a pending post for user {createPendingPost.PendingUserLogin}");
        }

        if (room.Claims.All(c => c.Character.Author.UserId != pendingUserId))
        {
            throw new HttpBadRequestException(new Dictionary<string, string>
            {
                [nameof(PendingPost.PendingUser)] = ValidationError.Invalid
            });
        }

        var pendingPostToCreate = factory.Create(createPendingPost, currentUserId, pendingUserId);

        var pendingPost = await repository.Create(pendingPostToCreate, cancellationToken);
        await producer.Send(EventType.RoomPendingCreated, pendingPost.Id);

        return pendingPost;
    }
}