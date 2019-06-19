using System;
using System.ComponentModel.DataAnnotations;
using BBCodeParser;

namespace DM.Web.Classic.Views.EditTopic
{
    public class EditTopicForm
    {
        public Guid ForumId { get; set; }
        public Guid TopicId { get; set; }

        [Required(ErrorMessage = "Введите название темы")]
        [StringLength(50, ErrorMessage = "В названии должно быть {1} или менее символов")]
        public string Title { get; set; }

        public string Text { get; set; }

        public IBbParser Parser { get; set; }
    }
}