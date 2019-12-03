using System;
using System.ComponentModel.DataAnnotations;
using BBCodeParser;

namespace DM.Web.Classic.Views.GameCommentaries.EditCommentary
{
    public class GameCommentaryEditForm
    {
        public Guid CommentaryId { get; set; }

        [Required(ErrorMessage = "Введите текст комментария")]
        public string Text { get; set; }

        public IBbParser Parser { get; set; }
    }
}