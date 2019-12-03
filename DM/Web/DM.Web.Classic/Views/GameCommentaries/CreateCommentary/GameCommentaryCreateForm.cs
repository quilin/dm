using System;
using System.ComponentModel.DataAnnotations;
using BBCodeParser;

namespace DM.Web.Classic.Views.GameCommentaries.CreateCommentary
{
    public class GameCommentaryCreateForm
    {
        public Guid GameId { get; set; }

        [Required(ErrorMessage = "Введите текст комментария")]
        public string Text { get; set; }

        public IBbParser Parser { get; set; }
    }
}