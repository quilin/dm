using System;
using System.Collections.Generic;
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
    /// <returns>Number of topic comments</returns>
    Task<int> Count(Guid topicId);

    /// <summary>
    /// Get comments list of the topic
    /// </summary>
    /// <param name="topicId">Topic id</param>
    /// <param name="paging">Paging data</param>
    /// <returns></returns>
    Task<IEnumerable<Comment>> Get(Guid topicId, PagingData paging);

    /// <summary>
    /// Get single comment by its identifier
    /// </summary>
    /// <param name="commentId">Commentary identifier</param>
    /// <returns>Found commentary</returns>
    Task<Comment> Get(Guid commentId);
}