using System;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.Community.BusinessProcesses.Reviews.Creating;

/// <summary>
/// Factory for review DAL model
/// </summary>
internal interface IReviewFactory
{
    /// <summary>
    /// Create review DAL model
    /// </summary>
    /// <param name="createReview"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Review Create(CreateReview createReview, Guid userId);
}