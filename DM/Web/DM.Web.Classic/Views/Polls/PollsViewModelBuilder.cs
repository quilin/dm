using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Polls;
using DM.Services.Community.BusinessProcesses.Polls.Reading;
using DM.Services.Core.Dto;
using DM.Services.Core.Implementation;

namespace DM.Web.Classic.Views.Polls
{
    public class PollsViewModelBuilder : IPollsViewModelBuilder
    {
        private readonly IPollReadingService readingService;
        private readonly IIntentionManager intentionManager;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IIdentityProvider identityProvider;

        public PollsViewModelBuilder(
            IPollReadingService readingService,
            IIntentionManager intentionManager,
            IDateTimeProvider dateTimeProvider,
            IIdentityProvider identityProvider)
        {
            this.readingService = readingService;
            this.intentionManager = intentionManager;
            this.dateTimeProvider = dateTimeProvider;
            this.identityProvider = identityProvider;
        }

        public async Task<PollsViewModel> Build(int entityNumber)
        {
            var (polls, paging) = await readingService.Get(new PagingQuery
            {
                Number = entityNumber
            }, false);

            return new PollsViewModel
            {
                Polls = polls.Select(Build),
                Paging = paging,
                CanCreate = intentionManager.IsAllowed(PollIntention.Create)
            };
        }

        public async Task<PollsViewModel> BuildActive()
        {
            var (polls, paging) = await readingService.Get(new PagingQuery
            {
                Size = int.MaxValue
            }, true);
            
            return new PollsViewModel
            {
                Polls = polls.Select(Build),
                Paging = paging,
                CanCreate = intentionManager.IsAllowed(PollIntention.Create)
            };
        }

        public PollViewModel Build(Poll poll)
        {
            var userId = identityProvider.Current.User.UserId;
            var options = poll.Options.Select(o => new PollOptionViewModel
            {
                Id = o.Id,
                Text = o.Text,
                VotesCount = o.UserIds.Count(),
                Voted = o.UserIds.Any(id => id == userId)
            }).ToArray();

            return new PollViewModel
            {
                Id = poll.Id,
                Title = poll.Title,

                IsClosed = poll.EndDate < dateTimeProvider.Now,
                IsVoted = options.Any(o => o.Voted),
                MaxVotesCount = Math.Max(options.Max(o => o.VotesCount), 1),
                TotalVotesCount = options.Sum(o => o.VotesCount),
                Options = options
            };
        }
    }
}