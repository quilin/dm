using System;
using System.Threading.Tasks;
using DM.Web.Classic.Views.Topic;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> Index(Guid topicId, int entityNumber)
        {
            var topicCommentariesViewModel = await topicViewModelBuilder.Build(topicId, entityNumber);
            return View("Topic", topicCommentariesViewModel);
        }
    }
}