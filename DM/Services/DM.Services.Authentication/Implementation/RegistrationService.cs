using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Factories;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Repositories;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.Eventing;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace DM.Services.Authentication.Implementation
{
    /// <inheritdoc />
    public class RegistrationService : IRegistrationService
    {
        private readonly IValidator<UserRegistration> validator;
        private readonly ISecurityManager securityManager;
        private readonly IUserFactory userFactory;
        private readonly IAuthenticationRepository repository;
        private readonly IMessagePublisher messagePublisher;
        private readonly MessagePublishConfiguration messagePublishConfiguration;

        /// <inheritdoc />
        public RegistrationService(
            IValidator<UserRegistration> validator,
            ISecurityManager securityManager,
            IUserFactory userFactory,
            IAuthenticationRepository repository,
            IMessagePublisher messagePublisher,
            IOptions<MessagePublishConfiguration> messagePublishConfiguration)
        {
            this.validator = validator;
            this.securityManager = securityManager;
            this.userFactory = userFactory;
            this.repository = repository;
            this.messagePublisher = messagePublisher;
            this.messagePublishConfiguration = messagePublishConfiguration.Value;
        }

        /// <inheritdoc />
        public async Task Register(UserRegistration registration)
        {
            await validator.ValidateAndThrowAsync(registration);

            var (hash, salt) = securityManager.GeneratePassword(registration.Password);
            var user = userFactory.Create(registration, salt, hash);
            await repository.CreateUserWithRegistrationToken(user);

            await messagePublisher.Publish(new InvokedEvent {Type = EventType.NewUser, EntityId = user.UserId},
                messagePublishConfiguration, "new.user");
        }
    }
}