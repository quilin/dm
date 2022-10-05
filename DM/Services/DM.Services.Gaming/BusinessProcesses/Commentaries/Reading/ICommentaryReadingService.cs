using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Reading;

/// <summary>
/// Service for reading game commentaries
/// </summary>
public interface ICommentaryReadingService
{
    /// <summary>
    /// Get list of game comments by game id
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="query">Paging query</param>
    /// <returns>Pair of comments list and paging data</returns>
    Task<(IEnumerable<Comment> comments, PagingResult paging, Game game)> Get(Guid gameId, PagingQuery query);

    /// <summary>
    /// Get game comment by id
    /// </summary>
    /// <param name="commentId">Commentary identifier</param>
    /// <returns>Found comment</returns>
    Task<Comment> Get(Guid commentId);

    /// <summary>
    /// Mark game comments as read
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task MarkAsRead(Guid gameId);
}