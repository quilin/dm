using System;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Authentication.Implementation;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users
{
    /// <inheritdoc />
    public class ActivationApiService : IActivationApiService
    {
        private readonly IActivationService activationService;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public ActivationApiService(
            IActivationService activationService,
            IMapper mapper)
        {
            this.activationService = activationService;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<Envelope<User>> Activate(Guid token)
        {
            var identity = await activationService.Activate(token);
            return new Envelope<User>(mapper.Map<User>(identity.User));
        }
    }
}