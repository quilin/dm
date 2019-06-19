﻿using DM.Services.Forum.BusinessProcesses.Topics.Creating;
using DM.Services.Forum.Dto.Input;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.Fora.CreateTopic;
using DM.Web.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.ForumControllers
{
    public class CreateTopicController : DmControllerBase
    {
        private readonly ITopicCreatingService topicCreatingService;

        public CreateTopicController(
            ITopicCreatingService topicCreatingService)
        {
            this.topicCreatingService = topicCreatingService;
        }

        [HttpPost, ValidationRequired]
        public ActionResult Create(CreateTopicForm createForm)
        {
            var createTopic = new CreateTopic
            {
                ForumTitle = createForm.ForumId,
                Title = createForm.Title,
                Text = createForm.Description
            };
            var topic = topicCreatingService.CreateTopic(createTopic).Result;
            return RedirectToAction("LastUnread", "Topic",
                new RouteValueDictionary{{"topicIdEncoded", topic.Id.EncodeToReadable(topic.Title)}});
        }
    }
}