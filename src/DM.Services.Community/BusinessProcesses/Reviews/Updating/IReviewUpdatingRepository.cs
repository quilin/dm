using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DM.Services.DataAccess.RelationalStorage;
using DbReview = DM.Services.DataAccess.BusinessObjects.Common.Review;

namespace DM.Services.Community.BusinessProcesses.Reviews.Updating;

/// <summary>
/// Storage for updating review
/// </summary>
internal interface IReviewUpdatingRepository
{
    /// <summary>
    /// Update review
    /// </summary>
    /// <param name="updateReview"></param>
    /// <returns></returns>
    Task<Review> Update(IUpdateBuilder<DbReview> updateReview);
}