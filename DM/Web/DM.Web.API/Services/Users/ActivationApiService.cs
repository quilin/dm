using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users
{
    /// <inheritdoc />
    public class ActivationApiService : IActivationApiService
    {
        /// <inheritdoc />
        public Task<Envelope<User>> Activate(Guid token)
        {
            throw new NotImplementedException();
        }
    }
}