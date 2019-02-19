using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;

namespace DM.Services.Forum.Repositories
{
    public interface ICommentRepository
    {
        Task<int> Count(Guid topicId);
        Task<IEnumerable<Comment>> Get(Guid topicId, PagingData paging);
    }
}