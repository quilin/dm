using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Games.Links;

namespace DM.Services.Gaming.BusinessProcesses.Games.Shared;

/// <inheritdoc />
internal class GameTagFactory : IGameTagFactory
{
    private readonly IGuidFactory guidFactory;

    /// <inheritdoc />
    public GameTagFactory(
        IGuidFactory guidFactory)
    {
        this.guidFactory = guidFactory;
    }

    /// <inheritdoc />
    public GameTag Create(Guid gameId, Guid tagId)
    {
        return new GameTag
        {
            GameTagId = guidFactory.Create(),
            GameId = gameId,
            TagId = tagId
        };
    }
}