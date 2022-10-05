using System.Linq;
using System.Net;
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
internal class BlacklistDeletingService : IBlacklistDeletingService
{
    private readonly IValidator<OperateBlacklistLink> validator;
    private readonly IUserRepository userRepository;
    private readonly IGameReadingService gameReadingService;
    private readonly IIntentionManager intentionManager;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IBlacklistDeletingRepository repository;
    private readonly IInvokedEventProducer invokedEventProducer;

    /// <inheritdoc />
    public BlacklistDeletingService(
        IValidator<OperateBlacklistLink> validator,
        IUserRepository userRepository,
        IGameReadingService gameReadingService,
        IIntentionManager intentionManager,
        IUpdateBuilderFactory updateBuilderFactory,
        IBlacklistDeletingRepository repository,
        IInvokedEventProducer invokedEventProducer)
    {
        this.validator = validator;
        this.userRepository = userRepository;
        this.gameReadingService = gameReadingService;
        this.intentionManager = intentionManager;
        this.updateBuilderFactory = updateBuilderFactory;
        this.repository = repository;
        this.invokedEventProducer = invokedEventProducer;
    }
        
    /// <inheritdoc />
    public async Task Delete(OperateBlacklistLink operateBlacklistLink)
    {
        await validator.ValidateAndThrowAsync(operateBlacklistLink);
        var game = await gameReadingService.GetGame(operateBlacklistLink.GameId);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, game);

        var (_, userId) = await userRepository.FindUserId(operateBlacklistLink.Login);
        var blacklistedLink = game.BlacklistedUsers.FirstOrDefault(u => u.UserId == userId);
        if (blacklistedLink == default)
        {
            throw new HttpException(HttpStatusCode.Conflict, "User is not blacklisted");
        }

        var updateBuilder = updateBuilderFactory.Create<BlackListLink>(blacklistedLink.LinkId).Delete();
        await repository.Delete(updateBuilder);
        await invokedEventProducer.Send(EventType.ChangedGame, game.Id);
    }
}