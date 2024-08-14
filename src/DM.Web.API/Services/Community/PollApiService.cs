using System;
using System.Linq;
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
internal class PollApiService : IPollApiService
{
    private readonly IPollReadingService readingService;
    private readonly IPollCreatingService creatingService;
    private readonly IPollVotingService votingService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public PollApiService(
        IPollReadingService readingService,
        IPollCreatingService creatingService,
        IPollVotingService votingService,
        IMapper mapper)
    {
        this.readingService = readingService;
        this.creatingService = creatingService;
        this.votingService = votingService;
        this.mapper = mapper;
    }
        
    /// <inheritdoc />
    public async Task<ListEnvelope<Poll>> Get(PollsQuery query)
    {
        var (polls, paging) = await readingService.Get(query, query.OnlyActive);
        return new ListEnvelope<Poll>(polls.Select(mapper.Map<Poll>), new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<Envelope<Poll>> Get(Guid id)
    {
        var poll = await readingService.Get(id);
        return new Envelope<Poll>(mapper.Map<Poll>(poll));
    }

    /// <inheritdoc />
    public async Task<Envelope<Poll>> Create(Poll poll)
    {
        var createPoll = mapper.Map<CreatePoll>(poll);
        var createdPoll = await creatingService.Create(createPoll);
        return new Envelope<Poll>(mapper.Map<Poll>(createdPoll));
    }

    /// <inheritdoc />
    public async Task<Envelope<Poll>> Vote(Guid pollId, Guid optionId)
    {
        var poll = await votingService.Vote(pollId, optionId);
        return new Envelope<Poll>(mapper.Map<Poll>(poll));
    }
}