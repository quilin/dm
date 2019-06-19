using System;
using DM.Web.Classic.Views.Shared.OpenPolls;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.PollControllers
{
    public class PollController : DmControllerBase
    {
        private readonly IPollService pollService;
        private readonly IPollsViewModelBuilder pollsViewModelBuilder;
        private readonly ICreatePollFormConverter createPollFormConverter;

        public PollController(
            IPollService pollService,
            IPollsViewModelBuilder pollsViewModelBuilder,
            ICreatePollFormConverter createPollFormConverter)
        {
            this.pollService = pollService;
            this.pollsViewModelBuilder = pollsViewModelBuilder;
            this.createPollFormConverter = createPollFormConverter;
        }

        [HttpGet]
        public ActionResult Create()
        {
            var createPollForm = new CreatePollForm();
            return View("OpenPolls/CreatePoll", createPollForm);
        }

        [HttpPost]
        public ActionResult Create(CreatePollForm createPollForm)
        {
            var pollCreateModel = createPollFormConverter.Convert(createPollForm);
            pollService.Create(pollCreateModel);
            return Redirect("/");
        }

        [HttpPost]
        public ActionResult Vote(Guid pollId, Guid optionId)
        {
            var poll = pollService.Vote(pollId, optionId);
            var pollViewModel = pollsViewModelBuilder.Build(poll);
            return PartialView("OpenPolls/Poll", pollViewModel);
        }
    }
}