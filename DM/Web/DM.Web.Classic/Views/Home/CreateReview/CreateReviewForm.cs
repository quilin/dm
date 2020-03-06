using System.ComponentModel.DataAnnotations;
using BBCodeParser;

namespace DM.Web.Classic.Views.Home.CreateReview
{
    public class CreateReviewForm
    {
        [Required(ErrorMessage = "Введите текст вашего отзыва")]
        public string Text { get; set; }

        public IBbParser Parser { get; set; }
    }
}