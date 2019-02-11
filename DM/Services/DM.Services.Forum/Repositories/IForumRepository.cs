using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Forum.Dto;

namespace DM.Services.Forum.Repositories
{
    public interface IForumRepository
    {
        Task<IEnumerable<ForaListItem>> SelectFora(Guid userId, ForumAccessPolicy accessPolicy);
        Task<ForaListItem> GetForum(string forumTitle, ForumAccessPolicy accessPolicy, Guid? userId = null);
    }
}