using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Games.Links;

namespace DM.Services.Gaming.BusinessProcesses.Readers.Subscribing;

/// <inheritdoc />
internal class ReaderFactory : IReaderFactory
{
    private readonly IGuidFactory guidFactory;

    /// <inheritdoc />
    public ReaderFactory(
        IGuidFactory guidFactory)
    {
        this.guidFactory = guidFactory;
    }
        
    /// <inheritdoc />
    public Reader Create(Guid userId, Guid gameId)
    {
        return new Reader
        {
            ReaderId = guidFactory.Create(),
            GameId = gameId,
            UserId = userId
        };
    }
}