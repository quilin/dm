using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Gaming
{
    /// <inheritdoc />
    public class LikeApiService : ILikeApiService
    {
        /// <inheritdoc />
        public Task<Envelope<User>> LikeComment(Guid commentId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task DislikeComment(Guid commentId)
        {
            throw new NotImplementedException();
        }
    }
}