using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using Comment = DM.Services.Common.Dto.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Reading;

/// <summary>
/// Service for reading forum commentaries
/// </summary>
public interface ICommentaryReadingService
{
    /// <summary>
    /// Get list of topic comments by topic id
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <param name="query">Paging query</param>
    /// <returns>Pair of comments list and paging data</returns>
    Task<(IEnumerable<Comment> comments, PagingResult paging)> Get(Guid topicId, PagingQuery query);

    /// <summary>
    /// Get topic comment by id
    /// </summary>
    /// <param name="commentId">Commentary identifier</param>
    /// <returns>Found comment</returns>
    Task<Comment> Get(Guid commentId);

    /// <summary>
    /// Mark all topic comments as read
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <returns></returns>
    Task MarkAsRead(Guid topicId);

    /// <summary>
    /// Mark all forum comments as read
    /// </summary>
    /// <param name="forumTitle">Forum title</param>
    /// <returns></returns>
    Task MarkAsRead(string forumTitle);
}