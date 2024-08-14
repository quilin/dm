using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Gaming.BusinessProcesses.Games.AssistantAssignment;

/// <inheritdoc />
internal class AssistantAssignmentTokenFactory : IAssistantAssignmentTokenFactory
{
    private readonly IGuidFactory guidFactory;
    private readonly IDateTimeProvider dateTimeProvider;

    /// <inheritdoc />
    public AssistantAssignmentTokenFactory(
        IGuidFactory guidFactory,
        IDateTimeProvider dateTimeProvider)
    {
        this.guidFactory = guidFactory;
        this.dateTimeProvider = dateTimeProvider;
    }

    /// <inheritdoc />
    public Token Create(Guid userId, Guid gameId)
    {
        return new Token
        {
            TokenId = guidFactory.Create(),
            UserId = userId,
            EntityId = gameId,
            Type = TokenType.AssistantAssignment,
            CreateDate = dateTimeProvider.Now,
            IsRemoved = false
        };
    }
}