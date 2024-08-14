using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Reading;

/// <summary>
/// Forum comments storage
/// </summary>
internal interface ICommentaryReadingRepository
{
    /// <summary>
    /// Count comments of the game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns>Number of game comments</returns>
    Task<int> Count(Guid gameId);

    /// <summary>
    /// Get comments list of the game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="paging">Paging data</param>
    /// <returns></returns>
    Task<IEnumerable<Comment>> Get(Guid gameId, PagingData paging);

    /// <summary>
    /// Get single comment by its identifier
    /// </summary>
    /// <param name="commentId">Commentary identifier</param>
    /// <returns>Found commentary</returns>
    Task<Comment> Get(Guid commentId);
}