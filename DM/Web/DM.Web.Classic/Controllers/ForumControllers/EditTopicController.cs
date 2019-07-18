using System;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Edit(Guid topicId)
        {
            var editTopicForm = await editTopicFormBuilder.Build(topicId);
            return View(editTopicForm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTopicForm form)
        {
            var updateTopic = new UpdateTopic
            {
                TopicId = form.TopicId,
                Title = form.Title,
                Text = form.Text
            };
            var topic = await topicUpdatingService.UpdateTopic(updateTopic);
            return RedirectToAction("Index", "Topic", new RouteValueDictionary
            {
                ["topicId"] = topic.Id.EncodeToReadable(topic.Title)
            });
        }
    }
}