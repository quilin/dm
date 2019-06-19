using System;
using DM.Web.Classic.Views.Topic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.ForumControllers
{
    public class TopicController : DmControllerBase
    {
        private readonly ITopicViewModelBuilder topicViewModelBuilder;
        private readonly IForumService forumService;
        private readonly IUnreadCounterService unreadCounterService;
        private readonly ICommentService commentService;

        public TopicController(
            ITopicViewModelBuilder topicViewModelBuilder,
            IForumService forumService,
            IUnreadCounterService unreadCounterService,
            ICommentService commentService,
            IUserProvider userProvider)
        {
            this.topicViewModelBuilder = topicViewModelBuilder;
            this.forumService = forumService;
            this.unreadCounterService = unreadCounterService;
            this.commentService = commentService;
        }

        public ActionResult LastUnread(string topicIdEncoded)
        {
            topicIdEncoded.TryDecodeFromReadableGuid(out var topicId);
            var entityNumber = 1;
            var totalCount = commentService.Count(topicId);
            if (totalCount > 0)
            {
                var unreadCount = unreadCounterService.GetCounter(topicId, EntryType.Message);
                entityNumber = Math.Min(totalCount - unreadCount + 1, totalCount);
            }

            return RedirectToAction("Index", new RouteValueDictionary
            {
                {"topicId", topicIdEncoded},
                {"entityNumber", entityNumber}
            });
        }

        public ActionResult Index(Guid topicId, int entityNumber)
        {
            var topicCommentariesViewModel = topicViewModelBuilder.Build(topicId, entityNumber);
            return View("Topic", topicCommentariesViewModel);
        }

        [HttpPost]
        public void CloseTopic(Guid topicId) => forumService.CloseTopic(topicId);

        [HttpPost]
        public void OpenTopic(Guid topicId) => forumService.OpenTopic(topicId);

        [HttpPost]
        public void AttachTopic(Guid topicId) => forumService.AttachTopic(topicId);

        [HttpPost]
        public void DetachTopic(Guid topicId) => forumService.DetachTopic(topicId);

        [HttpPost]
        public void RemoveTopic(Guid topicId) => forumService.RemoveTopic(topicId);

        [HttpPost]
        public void MoveTopic(Guid topicId, Guid forumId) => forumService.MoveTopic(topicId, forumId);
    }
}