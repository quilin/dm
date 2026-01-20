using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Polls.Reading;

namespace DM.Services.Community.BusinessProcesses.Polls.Voting;

/// <inheritdoc />
internal class PollVotingService(
    IPollReadingService readingService,
    IPollVotingRepository repository,
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider) : IPollVotingService
{
    /// <inheritdoc />
    public async Task<Poll> Vote(Guid pollId, Guid optionId, CancellationToken cancellationToken)
    {
        var poll = await readingService.Get(pollId, cancellationToken);
        intentionManager.ThrowIfForbidden(PollIntention.Vote, (poll, optionId));

        return await repository.Vote(pollId, optionId, identityProvider.Current.User.UserId, cancellationToken);
    }
}