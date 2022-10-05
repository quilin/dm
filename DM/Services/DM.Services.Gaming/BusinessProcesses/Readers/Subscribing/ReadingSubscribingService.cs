using System;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;

namespace DM.Services.Gaming.BusinessProcesses.Readers.Subscribing;

/// <inheritdoc />
internal class ReadingSubscribingService : IReadingSubscribingService
{
    private readonly IGameReadingService gameReadingService;
    private readonly IReaderFactory readerFactory;
    private readonly IReadingSubscribingRepository repository;
    private readonly IIntentionManager intentionManager;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public ReadingSubscribingService(
        IIdentityProvider identityProvider,
        IGameReadingService gameReadingService,
        IReaderFactory readerFactory,
        IReadingSubscribingRepository repository,
        IIntentionManager intentionManager)
    {
        this.gameReadingService = gameReadingService;
        this.readerFactory = readerFactory;
        this.repository = repository;
        this.intentionManager = intentionManager;
        this.identityProvider = identityProvider;
    }
        
    /// <inheritdoc />
    public async Task<GeneralUser> Subscribe(Guid gameId)
    {
        intentionManager.ThrowIfForbidden(GameIntention.Subscribe);
        var game = await gameReadingService.GetGame(gameId);
        intentionManager.ThrowIfForbidden(GameIntention.Subscribe, game);

        var identity = identityProvider.Current;
        var userId = identity.User.UserId;
        if (await repository.HasSubscription(userId, gameId))
        {
            throw new HttpException(HttpStatusCode.Conflict, "User already subscribed to this game");
        }

        var reader = readerFactory.Create(userId, gameId);
        await repository.Add(reader);
        return identity.User;
    }

    /// <inheritdoc />
    public async Task Unsubscribe(Guid gameId)
    {
        intentionManager.ThrowIfForbidden(GameIntention.Subscribe);
        var game = await gameReadingService.GetGame(gameId);
        intentionManager.ThrowIfForbidden(GameIntention.Unsubscribe, game);

        var userId = identityProvider.Current.User.UserId;
        if (!await repository.HasSubscription(userId, gameId))
        {
            throw new HttpException(HttpStatusCode.Conflict, "User is not subscribed to this game");
        }

        await repository.Delete(userId, gameId);
    }
}