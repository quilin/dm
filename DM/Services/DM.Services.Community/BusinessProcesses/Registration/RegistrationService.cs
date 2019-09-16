using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Community.BusinessProcesses.Registration.Confirmation;
using DM.Services.Community.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Registration
{
    /// <inheritdoc />
    public class RegistrationService : IRegistrationService
    {
        private readonly IValidator<UserRegistration> validator;
        private readonly ISecurityManager securityManager;
        private readonly IUserFactory userFactory;
        private readonly IRegistrationTokenFactory registrationTokenFactory;
        private readonly IRegistrationRepository repository;
        private readonly IRegistrationMailSender mailSender;
        private readonly IInvokedEventPublisher publisher;

        /// <inheritdoc />
        public RegistrationService(
            IValidator<UserRegistration> validator,
            ISecurityManager securityManager,
            IUserFactory userFactory,
            IRegistrationTokenFactory registrationTokenFactory,
            IRegistrationRepository repository,
            IRegistrationMailSender mailSender,
            IInvokedEventPublisher publisher)
        {
            this.validator = validator;
            this.securityManager = securityManager;
            this.userFactory = userFactory;
            this.registrationTokenFactory = registrationTokenFactory;
            this.repository = repository;
            this.mailSender = mailSender;
            this.publisher = publisher;
        }
        
        /// <inheritdoc />
        public async Task Register(UserRegistration registration)
        {
            await validator.ValidateAndThrowAsync(registration);

            var (hash, salt) = securityManager.GeneratePassword(registration.Password);
            var user = userFactory.Create(registration, salt, hash);
            var token = registrationTokenFactory.Create(user.UserId);

            await mailSender.Send(user.Email, user.Login, token.TokenId);
            await repository.AddUser(user, token);

            await publisher.Publish(EventType.NewUser, user.UserId);
        }
    }
}