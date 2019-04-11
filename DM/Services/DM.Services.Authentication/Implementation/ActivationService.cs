using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    /// <inheritdoc />
    public class ActivationService : IActivationService
    {
        /// <inheritdoc />
        public Task<IIdentity> Activate(Guid tokenId)
        {
            throw new NotImplementedException();
        }
    }
}