using System.Collections.Generic;
using DM.Web.Classic.Views.Home.News;
using DM.Web.Classic.Views.Home.Reviews;

namespace DM.Web.Classic.Views.Home
{
    public class HomeViewModel
    {
        public ReviewViewModel RandomReview { get; set; }
        public IEnumerable<NewsViewModel> News { get; set; }
    }
}