using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Forum.Dto;

namespace DM.Services.Forum.Repositories
{
    public interface ITopicRepository
    {
        Task<int> CountTopics(Guid forumId);
        Task<IEnumerable<TopicsListItem>> SelectTopics(Guid userId, Guid forumId, PagingData pagingData);
    }
}