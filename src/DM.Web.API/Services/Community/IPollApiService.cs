using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Web.API.Dto.Community;
using DM.Web.API.Dto.Contracts;

namespace DM.Web.API.Services.Community;

/// <summary>
/// API service for polls
/// </summary>
public interface IPollApiService
{
    /// <summary>
    /// Get polls
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ListEnvelope<Poll>> Get(PollsQuery query, CancellationToken cancellationToken);

    /// <summary>
    /// Get single poll
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Poll>> Get(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Create new poll
    /// </summary>
    /// <param name="poll"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Poll>> Create(Poll poll, CancellationToken cancellationToken);

    /// <summary>
    /// Vote for the poll option
    /// </summary>
    /// <param name="pollId">Poll identifier</param>
    /// <param name="optionId">Option identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Poll>> Vote(Guid pollId, Guid optionId, CancellationToken cancellationToken);
}