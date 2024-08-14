using System;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.Dto.Input;

namespace DM.Services.Forum.BusinessProcesses.Topics.Creating;

/// <summary>
/// Factory for topic DAL model
/// </summary>
internal interface ITopicFactory
{
    /// <summary>
    /// Create topic DAL for topic creation
    /// </summary>
    /// <param name="forumId">Forum identifier</param>
    /// <param name="userId">Author identifier</param>
    /// <param name="createTopic">Topic DTO</param>
    /// <returns></returns>
    ForumTopic Create(Guid forumId, Guid userId, CreateTopic createTopic);
}