using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.BusinessProcesses.Games.Shared;
using DM.Services.Gaming.Dto.Input;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Creating;

/// <inheritdoc />
internal class BlacklistCreatingService(
    IValidator<OperateBlacklistLink> validator,
    IGameReadingService gameReadingService,
    IIntentionManager intentionManager,
    IBlacklistLinkFactory factory,
    IUserRepository userRepository,
    IBlacklistCreatingRepository repository,
    IInvokedEventProducer producer) : IBlacklistCreatingService
{
    /// <inheritdoc />
    public async Task<GeneralUser> Create(
        OperateBlacklistLink operateBlacklistLink, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(operateBlacklistLink, cancellationToken);
        var game = await gameReadingService.GetGame(operateBlacklistLink.GameId, cancellationToken);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, game);

        var (_, userId) = await userRepository.FindUserId(operateBlacklistLink.Login, cancellationToken);
        if (game.BlacklistedUsers.Any(l => l.UserId == userId))
        {
            throw new HttpException(HttpStatusCode.Conflict, "User already blacklisted");
        }

        if (game.Master.UserId == userId || game.Nanny.UserId == userId)
        {
            throw new HttpException(HttpStatusCode.Forbidden,
                "Game owner and game moderator cannot be blacklisted");
        }

        var blackListLink = factory.Create(game.Id, userId);
        var blacklistedUser = await repository.Create(blackListLink, cancellationToken);
        await producer.Send(EventType.ChangedGame, game.Id);

        return blacklistedUser;
    }
}