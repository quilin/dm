using System;
using System.ComponentModel.DataAnnotations;
using BBCodeParser;

namespace DM.Web.Classic.Views.Shared.Commentaries
{
    public class CreateCommentaryForm
    {
        public Guid EntityId { get; set; }

        [Required(ErrorMessage = "Введите текст сообщения")]
        public string Text { get; set; }

        public IBbParser Parser { get; set; }
    }
}