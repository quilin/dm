using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    /// <inheritdoc />
    public class ActivationService : IActivationService
    {
        private readonly IRegistrationService registrationService;
        private readonly IAuthenticationService authenticationService;

        /// <inheritdoc />
        public ActivationService(
            IRegistrationService registrationService,
            IAuthenticationService authenticationService)
        {
            this.registrationService = registrationService;
            this.authenticationService = authenticationService;
        }
        
        /// <inheritdoc />
        public async Task<IIdentity> Activate(Guid tokenId)
        {
            var userId = await registrationService.Activate(tokenId);
            return await authenticationService.Authenticate(userId);
        }
    }
}