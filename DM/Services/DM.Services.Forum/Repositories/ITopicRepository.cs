using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Forum.Dto;

namespace DM.Services.Forum.Repositories
{
    public interface ITopicRepository
    {
        Task<int> Count(Guid forumId);
        Task<IEnumerable<TopicsListItem>> Get(Guid forumId, PagingData pagingData, bool attached);
    }
}