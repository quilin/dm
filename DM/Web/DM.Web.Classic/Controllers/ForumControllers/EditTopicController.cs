using System;
using DM.Web.Classic.Views.EditTopic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.ForumControllers
{
    public class EditTopicController : DmControllerBase
    {
        private readonly IEditTopicFormBuilder editTopicFormBuilder;
        private readonly IForumService forumService;

        public EditTopicController(
            IEditTopicFormBuilder editTopicFormBuilder,
            IForumService forumService
            )
        {
            this.editTopicFormBuilder = editTopicFormBuilder;
            this.forumService = forumService;
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
            var forumTopic = forumService.UpdateTopic(form.TopicId, form.Title, form.Text);
            return RedirectToAction("LastUnread", "Topic", new RouteValueDictionary
            {
                {"topicIdEncoded", forumTopic.ForumTopicId.EncodeToReadable(forumTopic.Title)}
            });
        }
    }
}