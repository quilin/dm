using System;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.Community.BusinessProcesses.Reviews.Deleting;

/// <summary>
/// Service for review removal
/// </summary>
public interface IReviewDeletingService
{
    /// <summary>
    /// Delete existing review
    /// </summary>
    /// <param name="id">Review identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid id, CancellationToken cancellationToken);
}