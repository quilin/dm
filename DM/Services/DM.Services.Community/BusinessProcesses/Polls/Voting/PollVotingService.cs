using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Polls.Reading;

namespace DM.Services.Community.BusinessProcesses.Polls.Voting;

/// <inheritdoc />
internal class PollVotingService : IPollVotingService
{
    private readonly IPollReadingService readingService;
    private readonly IPollVotingRepository repository;
    private readonly IIntentionManager intentionManager;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public PollVotingService(
        IPollReadingService readingService,
        IPollVotingRepository repository,
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider)
    {
        this.readingService = readingService;
        this.repository = repository;
        this.intentionManager = intentionManager;
        this.identityProvider = identityProvider;
    }
        
    /// <inheritdoc />
    public async Task<Poll> Vote(Guid pollId, Guid optionId)
    {
        var poll = await readingService.Get(pollId);
        intentionManager.ThrowIfForbidden(PollIntention.Vote, (poll, optionId));

        return await repository.Vote(pollId, optionId, identityProvider.Current.User.UserId);
    }
}