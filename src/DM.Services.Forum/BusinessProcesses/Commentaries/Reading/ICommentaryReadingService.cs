using System;
using System.Collections.Generic;
using System.Threading;
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
    /// <param name="cancellationToken"></param>
    /// <returns>Pair of comments list and paging data</returns>
    Task<(IEnumerable<Comment> comments, PagingResult paging)> Get(Guid topicId, PagingQuery query,
        CancellationToken cancellationToken);

    /// <summary>
    /// Get topic comment by id
    /// </summary>
    /// <param name="commentId">Commentary identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Found comment</returns>
    Task<Comment> Get(Guid commentId, CancellationToken cancellationToken);

    /// <summary>
    /// Mark all topic comments as read
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task MarkAsRead(Guid topicId, CancellationToken cancellationToken);

    /// <summary>
    /// Mark all forum comments as read
    /// </summary>
    /// <param name="forumTitle">Forum title</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task MarkAsRead(string forumTitle, CancellationToken cancellationToken);
}