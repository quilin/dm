using System.Threading;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DbReview = DM.Services.DataAccess.BusinessObjects.Common.Review;

namespace DM.Services.Community.BusinessProcesses.Reviews.Creating;

/// <summary>
/// Storage for review creating
/// </summary>
public interface IReviewCreatingRepository
{
    /// <summary>
    /// Create new review
    /// </summary>
    /// <param name="review"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Review> Create(DbReview review, CancellationToken cancellationToken);
}