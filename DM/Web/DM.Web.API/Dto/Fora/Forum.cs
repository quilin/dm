using DM.Services.Forum.Dto;

namespace DM.Web.API.Dto.Fora
{
    public class Forum
    {
        public Forum(ForaListItem forum)
        {
            Id = forum.Title;
            UnreadTopicsCount = forum.UnreadTopicsCount;
        }
        
        public string Id { get; }
        public int UnreadTopicsCount { get; }
    }
}