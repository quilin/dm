using System.Linq;
using System.Net;
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
internal class BlacklistCreatingService : IBlacklistCreatingService
{
    private readonly IValidator<OperateBlacklistLink> validator;
    private readonly IGameReadingService gameReadingService;
    private readonly IIntentionManager intentionManager;
    private readonly IBlacklistLinkFactory factory;
    private readonly IUserRepository userRepository;
    private readonly IBlacklistCreatingRepository repository;
    private readonly IInvokedEventProducer producer;

    /// <inheritdoc />
    public BlacklistCreatingService(
        IValidator<OperateBlacklistLink> validator,
        IGameReadingService gameReadingService,
        IIntentionManager intentionManager,
        IBlacklistLinkFactory factory,
        IUserRepository userRepository,
        IBlacklistCreatingRepository repository,
        IInvokedEventProducer producer)
    {
        this.validator = validator;
        this.gameReadingService = gameReadingService;
        this.intentionManager = intentionManager;
        this.factory = factory;
        this.userRepository = userRepository;
        this.repository = repository;
        this.producer = producer;
    }

    /// <inheritdoc />
    public async Task<GeneralUser> Create(OperateBlacklistLink operateBlacklistLink)
    {
        await validator.ValidateAndThrowAsync(operateBlacklistLink);
        var game = await gameReadingService.GetGame(operateBlacklistLink.GameId);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, game);

        var (_, userId) = await userRepository.FindUserId(operateBlacklistLink.Login);
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
        var blacklistedUser = await repository.Create(blackListLink);
        await producer.Send(EventType.ChangedGame, game.Id);

        return blacklistedUser;
    }
}