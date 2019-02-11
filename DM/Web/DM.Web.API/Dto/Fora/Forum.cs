using System.Collections.Generic;
using System.Linq;
using DM.Services.Forum.Dto;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Fora
{
    public class Forum
    {
        public Forum(ForaListItem forum)
        {
            Id = forum.Title;
            UnreadTopicsCount = forum.UnreadTopicsCount;
            Moderators = forum.Moderators.Select(m => new User(m));
        }
        
        public string Id { get; set; }
        public int UnreadTopicsCount { get; set; }
        public IEnumerable<User> Moderators { get; set; }
    }
}