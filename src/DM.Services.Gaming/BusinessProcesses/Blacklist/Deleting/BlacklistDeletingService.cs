using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.BusinessProcesses.Games.Shared;
using DM.Services.Gaming.Dto.Input;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Deleting;

/// <inheritdoc />
internal class BlacklistDeletingService(
    IValidator<OperateBlacklistLink> validator,
    IUserRepository userRepository,
    IGameReadingService gameReadingService,
    IIntentionManager intentionManager,
    IUpdateBuilderFactory updateBuilderFactory,
    IBlacklistDeletingRepository repository,
    IInvokedEventProducer invokedEventProducer) : IBlacklistDeletingService
{
    /// <inheritdoc />
    public async Task Delete(OperateBlacklistLink operateBlacklistLink, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(operateBlacklistLink, cancellationToken);
        var game = await gameReadingService.GetGame(operateBlacklistLink.GameId, cancellationToken);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, game);

        var (_, userId) = await userRepository.FindUserId(operateBlacklistLink.Login, cancellationToken);
        var blacklistedLink = game.BlacklistedUsers.FirstOrDefault(u => u.UserId == userId);
        if (blacklistedLink == default)
        {
            throw new HttpException(HttpStatusCode.Conflict, "User is not blacklisted");
        }

        var updateBuilder = updateBuilderFactory.Create<BlackListLink>(blacklistedLink.LinkId).Delete();
        await repository.Delete(updateBuilder, cancellationToken);
        await invokedEventProducer.Send(EventType.ChangedGame, game.Id);
    }
}