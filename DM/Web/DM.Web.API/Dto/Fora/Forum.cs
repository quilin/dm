using System.Collections.Generic;
using DM.Web.API.Dto.Common;

namespace DM.Web.API.Dto.Fora
{
    public class Forum
    {
        public string Id { get; set; }
        public int Unread { get; set; }
        public IEnumerable<User> Moderators { get; set; }
    }
}