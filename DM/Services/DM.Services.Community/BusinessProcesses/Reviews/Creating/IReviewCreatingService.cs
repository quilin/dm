using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;

namespace DM.Services.Community.BusinessProcesses.Reviews.Creating;

/// <summary>
/// Service for review creating
/// </summary>
public interface IReviewCreatingService
{
    /// <summary>
    /// Create new review
    /// </summary>
    /// <param name="createReview"></param>
    /// <returns></returns>
    Task<Review> Create(CreateReview createReview);
}