using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Community;
using DM.Web.API.Dto.Contracts;

namespace DM.Web.API.Services.Community
{
    /// <summary>
    /// API service for reviews
    /// </summary>
    public interface IReviewApiService
    {
        /// <summary>
        /// Get available reviews
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ListEnvelope<Review>> Get(PagingQuery query);

        /// <summary>
        /// Get random approved review
        /// </summary>
        /// <returns></returns>
        Task<Envelope<Review>> GetRandom();

        /// <summary>
        /// Get single review
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Envelope<Review>> Get(Guid id);

        /// <summary>
        /// Create new review
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        Task<Envelope<Review>> Create(Review review);

        /// <summary>
        /// Update existing review
        /// </summary>
        /// <param name="id"></param>
        /// <param name="review"></param>
        /// <returns></returns>
        Task<Envelope<Review>> Update(Guid id, Review review);

        /// <summary>
        /// Delete existing review
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
    }
}