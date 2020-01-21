using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.Forum.BusinessProcesses.Commentaries.Reading;
using DM.Services.Forum.BusinessProcesses.Topics.Deleting;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.BusinessProcesses.Topics.Updating;
using DM.Services.Forum.Dto.Input;
using DM.Web.Classic.Views.Topic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.ForumControllers
{
    public class TopicController : DmControllerBase
    {
        private readonly ITopicViewModelBuilder topicViewModelBuilder;
        private readonly ITopicUpdatingService topicUpdatingService;
        private readonly ITopicDeletingService topicDeletingService;
        private readonly ITopicReadingService topicReadingService;
        private readonly ICommentaryReadingService commentaryReadingService;

        public TopicController(
            ITopicViewModelBuilder topicViewModelBuilder,
            ITopicUpdatingService topicUpdatingService,
            ITopicDeletingService topicDeletingService,
            ITopicReadingService topicReadingService,
            ICommentaryReadingService commentaryReadingService)
        {
            this.topicViewModelBuilder = topicViewModelBuilder;
            this.topicUpdatingService = topicUpdatingService;
            this.topicDeletingService = topicDeletingService;
            this.topicReadingService = topicReadingService;
            this.commentaryReadingService = commentaryReadingService;
        }

        public async Task<IActionResult> Index(Guid topicId, int entityNumber)
        {
            var topicCommentariesViewModel = await topicViewModelBuilder.Build(topicId, entityNumber);
            return View("Topic", topicCommentariesViewModel);
        }

        public async Task<IActionResult> LastUnread(string topicIdEncoded)
        {
            topicIdEncoded.TryDecodeFromReadableGuid(out var topicId);
            var entityNumber = 1;
            var (_, paging) = await commentaryReadingService.Get(topicId, new PagingQuery {Size = 0});
            if (paging.TotalEntitiesCount > 0)
            {
                var topic = await topicReadingService.GetTopic(topicId);
                entityNumber = Math.Min(paging.TotalEntitiesCount - topic.UnreadCommentsCount + 1,
                    paging.TotalEntitiesCount);
            }

            return RedirectToAction("Index", new RouteValueDictionary
            {
                {"topicId", topicIdEncoded},
                {"entityNumber", entityNumber}
            });
        }

        [HttpPost]
        public Task CloseTopic(Guid topicId) => topicUpdatingService.UpdateTopic(new UpdateTopic
        {
            TopicId = topicId,
            Closed = true
        });

        [HttpPost]
        public Task OpenTopic(Guid topicId) => topicUpdatingService.UpdateTopic(new UpdateTopic
        {
            TopicId = topicId,
            Closed = false
        });

        [HttpPost]
        public Task AttachTopic(Guid topicId) => topicUpdatingService.UpdateTopic(new UpdateTopic
        {
            TopicId = topicId,
            Attached = true
        });

        [HttpPost]
        public Task DetachTopic(Guid topicId) => topicUpdatingService.UpdateTopic(new UpdateTopic
        {
            TopicId = topicId,
            Attached = false
        });

        [HttpPost]
        public Task RemoveTopic(Guid topicId) => topicDeletingService.DeleteTopic(topicId);

        [HttpPost]
        public Task MoveTopic(Guid topicId, string forumId) => topicUpdatingService.UpdateTopic(new UpdateTopic
        {
            TopicId = topicId,
            ForumTitle = forumId
        });
    }
}