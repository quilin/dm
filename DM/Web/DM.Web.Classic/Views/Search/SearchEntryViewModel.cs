using System;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Search
{
    public class SearchEntryViewModel
    {
        public Guid Id { get; set; }
        public UserViewModel Author { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}