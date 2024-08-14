using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Gaming.Dto;
using DM.Services.Gaming.Dto.Output;
using ServiceGame = DM.Services.Gaming.Dto.Output.Game;

namespace DM.Web.API.Dto.Games;

/// <summary>
/// Resolver for current user participation
/// </summary>
internal class GameParticipationResolver :
    IValueResolver<ServiceGame, Game, IEnumerable<GameParticipation>>,
    IValueResolver<GameExtended, Game, IEnumerable<GameParticipation>>
{
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public GameParticipationResolver(
        IIdentityProvider identityProvider)
    {
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public IEnumerable<GameParticipation> Resolve(
        ServiceGame source, Game destination, IEnumerable<GameParticipation> destMember, ResolutionContext context) =>
        Flatten(source.Participation(identityProvider.Current.User.UserId));

    /// <inheritdoc />
    public IEnumerable<GameParticipation> Resolve(
        GameExtended source, Game destination, IEnumerable<GameParticipation> destMember, ResolutionContext context) =>
        Flatten(source.Participation(identityProvider.Current.User.UserId));

    private static IEnumerable<GameParticipation> Flatten(GameParticipation participation) =>
        Enum.GetValues(typeof(GameParticipation))
            .Cast<GameParticipation>()
            .Where(p => (p & participation) != GameParticipation.None);
}