using System.Threading.Tasks;
using DM.Services.Forum.BusinessProcesses.Commentaries.Reading;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.Fora;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.ForumControllers
{
    public class ForumController : Controller
    {
        private readonly IForumReadingService forumReadingService;
        private readonly ICommentaryReadingService commentaryReadingService;
        private readonly IForumViewModelBuilder forumViewModelBuilder;

        public ForumController(
            IForumReadingService forumReadingService,
            ICommentaryReadingService commentaryReadingService,
            IForumViewModelBuilder forumViewModelBuilder)
        {
            this.forumReadingService = forumReadingService;
            this.commentaryReadingService = commentaryReadingService;
            this.forumViewModelBuilder = forumViewModelBuilder;
        }

        public async Task<IActionResult> Index(string forumTitle, int entityNumber, string category)
        {
            if (string.IsNullOrEmpty(forumTitle))
            {
                return string.IsNullOrEmpty(category)
                    ? RedirectToActionPermanent("Index", "Forum", new RouteValueDictionary {{"forumTitle", "Общий"}})
                    : RedirectToActionPermanent("Index", "Forum", new RouteValueDictionary {{"forumTitle", category}});
            }

            var forum = await forumReadingService.GetSingleForum(forumTitle);

            if (!Request.IsAjaxRequest())
            {
                var forumViewModel = await forumViewModelBuilder.Build(forum, entityNumber);
                return View("~/Views/Fora/Forum.cshtml", forumViewModel);
            }

            var topicViewModels = await forumViewModelBuilder.BuildList(forum, entityNumber);
            return View("~/Views/Fora/ForumTopicsList.cshtml", topicViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead(string forumTitle)
        {
            await commentaryReadingService.MarkAsRead(forumTitle);
            return NoContent();
        }
    }
}