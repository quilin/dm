using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Community;
using DM.Web.API.Dto.Contracts;

namespace DM.Web.API.Services.Community
{
    /// <inheritdoc />
    public class ReviewApiService : IReviewApiService
    {
        /// <inheritdoc />
        public Task<ListEnvelope<Review>> Get(PagingQuery query)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<Envelope<Review>> GetRandom()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<Envelope<Review>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<Envelope<Review>> Create(Review review)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<Envelope<Review>> Update(Guid id, Review review)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}