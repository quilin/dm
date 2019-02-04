using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Forum.Dto;

namespace DM.Services.Forum.Implementation
{
    public interface IForumService
    {
        Task<IEnumerable<ForaListItem>> GetForaList();

        Task<ForaListItem> GetForum(string forumTitle);

        Task<(ForumTitle Forum, IEnumerable<TopicsListItem> Topics)> GetTopicsList(string forumTitle, int entityNumber);
    }
}