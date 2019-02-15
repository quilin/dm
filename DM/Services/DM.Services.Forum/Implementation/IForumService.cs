using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Forum.Dto;

namespace DM.Services.Forum.Implementation
{
    public interface IForumService
    {
        Task<IEnumerable<ForaListItem>> GetForaList();
        Task<ForaListItem> GetForum(string forumTitle);
        Task<(IEnumerable<TopicsListItem> topics, PagingData paging)> GetTopicsList(string forumTitle, int entityNumber);
        Task<IEnumerable<TopicsListItem>> GetAttachedTopics(string forumTitle);
        Task<IEnumerable<GeneralUser>> GetModerators(string forumTitle);
    }
}