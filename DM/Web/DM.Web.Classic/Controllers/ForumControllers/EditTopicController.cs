using System;
using DM.Services.Forum.BusinessProcesses.Topics.Updating;
using DM.Services.Forum.Dto.Input;
using DM.Web.Classic.Views.EditTopic;
using DM.Web.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.ForumControllers
{
    public class EditTopicController : DmControllerBase
    {
        private readonly IEditTopicFormBuilder editTopicFormBuilder;
        private readonly ITopicUpdatingService topicUpdatingService;

        public EditTopicController(
            IEditTopicFormBuilder editTopicFormBuilder,
            ITopicUpdatingService topicUpdatingService)
        {
            this.editTopicFormBuilder = editTopicFormBuilder;
            this.topicUpdatingService = topicUpdatingService;
        }

        [HttpGet]
        public ActionResult Edit(Guid topicId)
        {
            var editTopicForm = editTopicFormBuilder.Build(topicId);
            return View(editTopicForm);
        }

        [HttpPost]
        public ActionResult Edit(EditTopicForm form)
        {
            var updateTopic = new UpdateTopic
            {
                TopicId = form.TopicId,
                Title = form.Title,
                Text = form.Text
            };
            var topic = topicUpdatingService.UpdateTopic(updateTopic).Result;
            return RedirectToAction("LastUnread", "Topic", new RouteValueDictionary
            {
                {"topicIdEncoded", topic.Id.EncodeToReadable(topic.Title)}
            });
        }
    }
}