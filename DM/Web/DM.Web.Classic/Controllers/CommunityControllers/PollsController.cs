using System;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Polls.Creating;
using DM.Services.Community.BusinessProcesses.Polls.Voting;
using DM.Services.Core.Implementation;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.Polls;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CommunityControllers
{
    public class PollsController : Controller
    {
        private readonly IPollsViewModelBuilder viewModelBuilder;
        private readonly IPollCreatingService creatingService;
        private readonly IPollVotingService votingService;
        private readonly IDateTimeProvider dateTimeProvider;

        public PollsController(
            IPollsViewModelBuilder viewModelBuilder,
            IPollCreatingService creatingService,
            IPollVotingService votingService,
            IDateTimeProvider dateTimeProvider)
        {
            this.viewModelBuilder = viewModelBuilder;
            this.creatingService = creatingService;
            this.votingService = votingService;
            this.dateTimeProvider = dateTimeProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int entityNumber)
        {
            var viewModel = await viewModelBuilder.Build(entityNumber);
            return Request.IsAjaxRequest()
                ? (IActionResult) PartialView("Polls", viewModel)
                : View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var createPollForm = new CreatePollForm();
            return View("CreatePoll", createPollForm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePollForm createPollForm)
        {
            await creatingService.Create(new CreatePoll
            {
                Title = createPollForm.Text,
                EndDate = dateTimeProvider.Now + TimeSpan.FromDays(10),
                Options = createPollForm.Options.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries)
            });
            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Vote(Guid pollId, Guid optionId)
        {
            var poll = await votingService.Vote(pollId, optionId);
            var pollViewModel = viewModelBuilder.Build(poll);
            return PartialView("~/Views/Polls/Poll.cshtml", pollViewModel);
        }
    }
}