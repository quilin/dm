using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DbReview = DM.Services.DataAccess.BusinessObjects.Common.Review;

namespace DM.Services.Community.BusinessProcesses.Reviews.Creating;

/// <summary>
/// Storage for review creating
/// </summary>
internal interface IReviewCreatingRepository
{
    /// <summary>
    /// Create new review
    /// </summary>
    /// <param name="review"></param>
    /// <returns></returns>
    Task<Review> Create(DbReview review);
}