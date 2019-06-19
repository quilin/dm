using DM.Web.Classic.Views.Votes;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.RatingControllers
{
    public class VotesController : DmControllerBase
    {
        private readonly IVotesListViewModelBuilder votesListViewModelBuilder;

        public VotesController(
            IVotesListViewModelBuilder votesListViewModelBuilder
            )
        {
            this.votesListViewModelBuilder = votesListViewModelBuilder;
        }

        public IActionResult UserVotes(string login)
        {
            var votesListViewModel = votesListViewModelBuilder.BuildForVoter(login);
            ViewData["VotesListTitle"] = "Отданные голоса";
            return View("VotesList", votesListViewModel);
        }

        public IActionResult UserRating(string login)
        {
            var votesListViewModel = votesListViewModelBuilder.BuildForUser(login);
            ViewData["VotesListTitle"] = "Рейтинг";
            return View("VotesList", votesListViewModel);
        }
    }
}