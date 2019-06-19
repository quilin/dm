using DM.Web.Classic.Views.Fora.CreateTopic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.ForumControllers
{
    public class CreateTopicController : DmControllerBase
    {
        private readonly IForumService forumService;

        public CreateTopicController(
            IForumService forumService
            )
        {
            this.forumService = forumService;
        }

        [HttpPost, ValidationRequired]
        public ActionResult Create(CreateTopicForm createForm)
        {
            var topic = forumService.CreateTopic(createForm.ForumId, createForm.Title, createForm.Description, createForm.FirstPost);
            return RedirectToAction("LastUnread", "Topic", new RouteValueDictionary{{"topicIdEncoded", topic.ForumTopicId.EncodeToReadable(topic.Title)}});
        }
    }
}