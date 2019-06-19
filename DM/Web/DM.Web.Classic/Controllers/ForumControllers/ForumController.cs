using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.Fora;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.ForumControllers
{
    public class ForumController : DmControllerBase
    {
        private readonly IForumReadingService forumReadingService;
        private readonly IForumViewModelBuilder forumViewModelBuilder;

        public ForumController(
            IForumReadingService forumReadingService,
            IForumViewModelBuilder forumViewModelBuilder)
        {
            this.forumReadingService = forumReadingService;
            this.forumViewModelBuilder = forumViewModelBuilder;
        }

        public ActionResult Index(string forumTitle, int entityNumber, string category)
        {
            if (string.IsNullOrEmpty(forumTitle))
            {
                return string.IsNullOrEmpty(category)
                    ? RedirectToActionPermanent("Index", "Forum", new RouteValueDictionary {{"forumTitle", "Общий"}})
                    : RedirectToActionPermanent("Index", "Forum", new RouteValueDictionary {{"forumTitle", category}});
            }

            var forum = forumReadingService.GetSingleForum(forumTitle).Result;

            if (!Request.IsAjaxRequest())
            {
                var forumViewModel = forumViewModelBuilder.Build(forum, entityNumber);
                return View("~/Views/Fora/Forum.cshtml", forumViewModel);
            }

            var topicViewModels = forumViewModelBuilder.BuildList(forum, entityNumber);
            return View("~/Views/Fora/ForumTopicsList.cshtml", topicViewModels);
        }
    }
}