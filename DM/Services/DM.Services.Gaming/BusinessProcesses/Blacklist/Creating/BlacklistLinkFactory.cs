using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Games.Links;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Creating;

/// <inheritdoc />
internal class BlacklistLinkFactory : IBlacklistLinkFactory
{
    private readonly IGuidFactory guidFactory;

    /// <inheritdoc />
    public BlacklistLinkFactory(
        IGuidFactory guidFactory)
    {
        this.guidFactory = guidFactory;
    }
        
    /// <inheritdoc />
    public BlackListLink Create(Guid gameId, Guid userId)
    {
        return new BlackListLink
        {
            BlackListLinkId = guidFactory.Create(),
            GameId = gameId,
            UserId = userId
        };
    }
}