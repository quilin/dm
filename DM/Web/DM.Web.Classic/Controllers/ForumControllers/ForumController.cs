using System;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.Fora;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.ForumControllers
{
    public class ForumController : DmControllerBase
    {
        private readonly Views.ForaList.IForaListViewModelBuilder generalForaListViewModelBuilder;
        private readonly IForumService forumService;
        private readonly IIntentionsManager intentionsManager;
        private readonly IForumViewModelBuilder forumViewModelBuilder;
        private readonly IUnreadCounterService unreadCounterService;

        public ForumController(
            Views.ForaList.IForaListViewModelBuilder generalForaListViewModelBuilder,
            IForumService forumService,
            IIntentionsManager intentionsManager,
            IForumViewModelBuilder forumViewModelBuilder,
            IUnreadCounterService unreadCounterService)
        {
            this.generalForaListViewModelBuilder = generalForaListViewModelBuilder;
            this.forumService = forumService;
            this.intentionsManager = intentionsManager;
            this.forumViewModelBuilder = forumViewModelBuilder;
            this.unreadCounterService = unreadCounterService;
        }

        public ActionResult Index(string forumTitle, int entityNumber, string category)
        {
            if (string.IsNullOrEmpty(forumTitle))
            {
                return string.IsNullOrEmpty(category)
                    ? RedirectToActionPermanent("List", "Forum")
                    : RedirectToActionPermanent("Index", "Forum", new RouteValueDictionary {{"forumTitle", category}});
            }

            var forum = forumService.ReadForum(forumTitle);
            intentionsManager.ThrowIfForbidden(ForumIntention.Read, forum);

            if (!Request.IsAjaxRequest())
            {
                var forumViewModel = forumViewModelBuilder.Build(forum, entityNumber);
                return View("~/Views/Fora/Forum.cshtml", forumViewModel);
            }

            var topicViewModels = forumViewModelBuilder.BuildList(forum, entityNumber);
            return View("~/Views/Fora/ForumTopicsList.cshtml", topicViewModels);
        }

        [HttpPost]
        public ActionResult MarkAllAsRead(Guid forumId)
        {
            var forum = forumService.ReadForum(forumId);
            intentionsManager.ThrowIfForbidden(CommonIntention.MarkMessagesAsRead);

            unreadCounterService.FlushAll(forumId, EntryType.Message);
            return RedirectToAction("Index", new RouteValueDictionary{{"forumTitle", forum.Title}});
        }

        public ActionResult List()
        {
            var foraListViewModel = generalForaListViewModelBuilder.Build();
            return View("~/Views/ForaList/ForaList.cshtml", foraListViewModel);
        }
    }
}