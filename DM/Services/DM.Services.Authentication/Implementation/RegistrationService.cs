using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Factories;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Repositories;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;

namespace DM.Services.Authentication.Implementation
{
    /// <inheritdoc />
    public class RegistrationService : IRegistrationService
    {
        private readonly IValidator<UserRegistration> validator;
        private readonly ISecurityManager securityManager;
        private readonly IUserFactory userFactory;
        private readonly IRegistrationTokenFactory registrationTokenFactory;
        private readonly IAuthenticationRepository repository;
        private readonly IInvokedEventPublisher publisher;

        /// <inheritdoc />
        public RegistrationService(
            IValidator<UserRegistration> validator,
            ISecurityManager securityManager,
            IUserFactory userFactory,
            IRegistrationTokenFactory registrationTokenFactory,
            IAuthenticationRepository repository,
            IInvokedEventPublisher publisher)
        {
            this.validator = validator;
            this.securityManager = securityManager;
            this.userFactory = userFactory;
            this.registrationTokenFactory = registrationTokenFactory;
            this.repository = repository;
            this.publisher = publisher;
        }

        /// <inheritdoc />
        public async Task Register(UserRegistration registration)
        {
            await validator.ValidateAndThrowAsync(registration);

            var (hash, salt) = securityManager.GeneratePassword(registration.Password);
            var user = userFactory.Create(registration, salt, hash);
            var token = registrationTokenFactory.Create(user.UserId);
            await repository.AddUser(user, token);

            await publisher.Publish(EventType.NewUser, user.UserId);
        }
    }
}