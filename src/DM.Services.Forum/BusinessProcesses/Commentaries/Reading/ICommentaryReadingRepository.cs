using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Reading;

/// <summary>
/// Forum comments storage
/// </summary>
internal interface ICommentaryReadingRepository
{
    /// <summary>
    /// Count comments of the topic
    /// </summary>
    /// <param name="topicId">Topic id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Number of topic comments</returns>
    Task<int> Count(Guid topicId, CancellationToken cancellationToken);

    /// <summary>
    /// Get comments list of the topic
    /// </summary>
    /// <param name="topicId">Topic id</param>
    /// <param name="paging">Paging data</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Comment>> Get(Guid topicId, PagingData paging, CancellationToken cancellationToken);

    /// <summary>
    /// Get single comment by its identifier
    /// </summary>
    /// <param name="commentId">Commentary identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Found commentary</returns>
    Task<Comment> Get(Guid commentId, CancellationToken cancellationToken);
}