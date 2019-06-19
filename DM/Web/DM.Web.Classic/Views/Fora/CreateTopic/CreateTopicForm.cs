using System.ComponentModel.DataAnnotations;
using BBCodeParser;

namespace DM.Web.Classic.Views.Fora.CreateTopic
{
    public class CreateTopicForm
    {
        public string ForumId { get; set; }

        [Required(ErrorMessage = "Введите название темы")]
        [StringLength(50, ErrorMessage = "В названии должно быть {1} или менее символов")]
        public string Title { get; set; }

        public string Description { get; set; }
        public IBbParser Parser { get; set; }
    }
}