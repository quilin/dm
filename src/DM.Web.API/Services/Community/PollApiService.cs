using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Polls.Creating;
using DM.Services.Community.BusinessProcesses.Polls.Reading;
using DM.Services.Community.BusinessProcesses.Polls.Voting;
using DM.Web.API.Dto.Community;
using DM.Web.API.Dto.Contracts;
using Poll = DM.Web.API.Dto.Community.Poll;

namespace DM.Web.API.Services.Community;

/// <inheritdoc />
internal class PollApiService(
    IPollReadingService readingService,
    IPollCreatingService creatingService,
    IPollVotingService votingService,
    IMapper mapper) : IPollApiService
{
    /// <inheritdoc />
    public async Task<ListEnvelope<Poll>> Get(PollsQuery query, CancellationToken cancellationToken)
    {
        var (polls, paging) = await readingService.Get(query, query.OnlyActive, cancellationToken);
        return new ListEnvelope<Poll>(polls.Select(mapper.Map<Poll>), new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<Envelope<Poll>> Get(Guid id, CancellationToken cancellationToken)
    {
        var poll = await readingService.Get(id, cancellationToken);
        return new Envelope<Poll>(mapper.Map<Poll>(poll));
    }

    /// <inheritdoc />
    public async Task<Envelope<Poll>> Create(Poll poll, CancellationToken cancellationToken)
    {
        var createPoll = mapper.Map<CreatePoll>(poll);
        var createdPoll = await creatingService.Create(createPoll, cancellationToken);
        return new Envelope<Poll>(mapper.Map<Poll>(createdPoll));
    }

    /// <inheritdoc />
    public async Task<Envelope<Poll>> Vote(Guid pollId, Guid optionId, CancellationToken cancellationToken)
    {
        var poll = await votingService.Vote(pollId, optionId, cancellationToken);
        return new Envelope<Poll>(mapper.Map<Poll>(poll));
    }
}