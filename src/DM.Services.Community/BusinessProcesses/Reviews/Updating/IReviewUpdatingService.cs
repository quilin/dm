using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;

namespace DM.Services.Community.BusinessProcesses.Reviews.Updating;

/// <summary>
/// Service for updating reviews
/// </summary>
public interface IReviewUpdatingService
{
    /// <summary>
    /// Update existing review
    /// </summary>
    /// <param name="updateReview"></param>
    /// <returns></returns>
    Task<Review> Update(UpdateReview updateReview);
}