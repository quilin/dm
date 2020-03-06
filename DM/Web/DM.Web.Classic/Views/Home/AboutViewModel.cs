using DM.Web.Classic.Views.Home.CreateReview;
using DM.Web.Classic.Views.Home.Reviews;

namespace DM.Web.Classic.Views.Home
{
    public class AboutViewModel
    {
        public bool CanCreate { get; set; }
        public CreateReviewForm CreateReviewForm { get; set; }
        public ReviewViewModel[] Reviews { get; set; }
    }
}