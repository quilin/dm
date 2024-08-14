using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Games;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.MessageQueuing.GeneralBus;

namespace DM.Services.Gaming.BusinessProcesses.Games.AssistantAssignment;

/// <inheritdoc />
internal class AssignmentService : IAssignmentService
{
    private readonly IIdentityProvider identityProvider;
    private readonly IAssistantAssignmentTokenFactory tokenFactory;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IAssignmentRepository repository;
    private readonly IInvokedEventProducer producer;

    /// <inheritdoc />
    public AssignmentService(
        IIdentityProvider identityProvider,
        IAssistantAssignmentTokenFactory tokenFactory,
        IUpdateBuilderFactory updateBuilderFactory,
        IAssignmentRepository repository,
        IInvokedEventProducer producer)
    {
        this.identityProvider = identityProvider;
        this.tokenFactory = tokenFactory;
        this.updateBuilderFactory = updateBuilderFactory;
        this.repository = repository;
        this.producer = producer;
    }

    /// <inheritdoc />
    public async Task CreateAssignment(Guid gameId, Guid userId)
    {
        var updates = (await repository.FindAssignments(gameId))
            .Select(id => updateBuilderFactory.Create<Token>(id).Field(t => t.IsRemoved, true));
        var token = tokenFactory.Create(userId, gameId);
        await repository.InvalidateAndCreate(updates, token);
        await producer.Send(EventType.AssignmentRequestCreated, token.TokenId);
    }

    /// <inheritdoc />
    public Task AcceptAssignment(Guid tokenId) => Assign(tokenId, true);

    /// <inheritdoc />
    public Task RejectAssignment(Guid tokenId) => Assign(tokenId, false);

    private async Task Assign(Guid tokenId, bool accept)
    {
        var userId = identityProvider.Current.User.UserId;
        var gameId = await repository.FindGameToAssign(tokenId, userId);
        if (!gameId.HasValue)
        {
            throw new HttpException(HttpStatusCode.Gone,
                "Activation token is invalid! Address the technical support for further assistance");
        }

        var updateToken = updateBuilderFactory.Create<Token>(tokenId).Field(t => t.IsRemoved, true);
        var updateGame = updateBuilderFactory.Create<Game>(gameId.Value);
        var eventType = EventType.AssignmentRequestRejected;
        if (accept)
        {
            updateGame = updateGame.Field(g => g.AssistantId, userId);
            eventType = EventType.AssignmentRequestAccepted;
        }

        await repository.AssignAssistant(updateGame, updateToken);
        await producer.Send(eventType, tokenId);
    }
}