using System.Linq;
using AutoMapper;
using DM.Services.Authentication.Implementation.UserIdentity;
using DtoPollOption = DM.Services.Community.BusinessProcesses.Polls.Reading.PollOption;

namespace DM.Web.API.Dto.Community;

/// <inheritdoc />
internal class PollParticipationResolver : IValueResolver<DtoPollOption, PollOption, bool?>
{
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public PollParticipationResolver(
        IIdentityProvider identityProvider)
    {
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public bool? Resolve(DtoPollOption source, PollOption destination, bool? destMember, ResolutionContext context)
    {
        var currentUser = identityProvider.Current.User;
        return currentUser.IsAuthenticated
            ? source.UserIds.Contains(currentUser.UserId)
            : null;
    }
}