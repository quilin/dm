using System;
using System.Collections.Generic;

namespace DM.Web.API.Dto.Common
{
    public class Comment
    {
        public string Id { get; set; }
        public User Author { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string Text { get; set; }
        public IEnumerable<User> Likes { get; set; }
    }
}