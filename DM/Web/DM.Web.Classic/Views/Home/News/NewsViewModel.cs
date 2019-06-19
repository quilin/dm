using System;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Home.News
{
    public class NewsViewModel
    {
        public Guid TopicId { get; set; }
        public DateTime CreateDate { get; set; }

        public UserViewModel Author { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public int UnreadComments { get; set; }
    }
}