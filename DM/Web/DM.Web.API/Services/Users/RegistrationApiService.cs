using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users
{
    /// <inheritdoc />
    public class RegistrationApiService : IRegistrationApiService
    {
        private readonly IRegistrationService registrationService;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public RegistrationApiService(
            IRegistrationService registrationService,
            IMapper mapper)
        {
            this.registrationService = registrationService;
            this.mapper = mapper;
        }
        
        /// <inheritdoc />
        public Task Register(Registration registration) =>
            registrationService.Register(mapper.Map<UserRegistration>(registration));
    }
}