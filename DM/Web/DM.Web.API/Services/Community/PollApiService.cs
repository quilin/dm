using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Polls.Creating;
using DM.Services.Community.BusinessProcesses.Polls.Reading;
using DM.Web.API.Dto.Community;
using DM.Web.API.Dto.Contracts;
using Poll = DM.Web.API.Dto.Community.Poll;

namespace DM.Web.API.Services.Community
{
    /// <inheritdoc />
    public class PollApiService : IPollApiService
    {
        private readonly IPollReadingService readingService;
        private readonly IPollCreatingService creatingService;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public PollApiService(
            IPollReadingService readingService,
            IPollCreatingService creatingService,
            IMapper mapper)
        {
            this.readingService = readingService;
            this.creatingService = creatingService;
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
    }
}