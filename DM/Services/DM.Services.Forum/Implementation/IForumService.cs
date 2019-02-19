using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;
using DM.Services.Forum.Dto;

namespace DM.Services.Forum.Implementation
{
    public interface IForumService
    {
        Task<IEnumerable<Dto.Forum>> GetForaList();
        Task<Dto.Forum> GetForum(string forumTitle);

        Task<IEnumerable<GeneralUser>> GetModerators(string forumTitle);

        Task<(IEnumerable<Topic> topics, PagingData paging)> GetTopicsList(string forumTitle, int entityNumber);
        Task<IEnumerable<Topic>> GetAttachedTopics(string forumTitle);
        Task<Topic> GetTopic(Guid topicId);

        Task<(IEnumerable<Comment> comments, PagingData paging)> GetCommentsList(Guid topicId, int entityNumber);
    }
}