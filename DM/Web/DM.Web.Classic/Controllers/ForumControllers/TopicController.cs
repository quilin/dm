using System;
using System.Threading.Tasks;
using DM.Web.Classic.Views.Topic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.ForumControllers
{
    public class TopicController : DmControllerBase
    {
        private readonly ITopicViewModelBuilder topicViewModelBuilder;

        public TopicController(
            ITopicViewModelBuilder topicViewModelBuilder)
        {
            this.topicViewModelBuilder = topicViewModelBuilder;
        }

        public ActionResult LastUnread(string topicIdEncoded)
        {
            return RedirectToAction("Index", new RouteValueDictionary
            {
                {"topicId", topicIdEncoded},
                {"entityNumber", 1}
            });
        }

        public async Task<IActionResult> Index(Guid topicId, int entityNumber)
        {
            var topicCommentariesViewModel = await topicViewModelBuilder.Build(topicId, entityNumber);
            return View("Topic", topicCommentariesViewModel);
        }
    }
}