using System;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Factories;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Repositories;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
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
        private readonly IRegistrationRepository repository;
        private readonly IInvokedEventPublisher publisher;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <inheritdoc />
        public RegistrationService(
            IValidator<UserRegistration> validator,
            ISecurityManager securityManager,
            IUserFactory userFactory,
            IRegistrationTokenFactory registrationTokenFactory,
            IRegistrationRepository repository,
            IInvokedEventPublisher publisher,
            IDateTimeProvider dateTimeProvider)
        {
            this.validator = validator;
            this.securityManager = securityManager;
            this.userFactory = userFactory;
            this.registrationTokenFactory = registrationTokenFactory;
            this.repository = repository;
            this.publisher = publisher;
            this.dateTimeProvider = dateTimeProvider;
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

        /// <inheritdoc />
        public async Task<Guid> Activate(Guid tokenId)
        {
            var userId = await repository.FindUserToActivate(tokenId, dateTimeProvider.Ago(TimeSpan.FromDays(2)));
            if (userId == default)
            {
                throw new HttpException(HttpStatusCode.Gone,
                    "Activation token is invalid! Address the technical support for further assistance");
            }

            await repository.ActivateUser(
                new UpdateBuilder<User>(userId).Field(u => u.Activated, true),
                new UpdateBuilder<Token>(tokenId).Field(t => t.IsRemoved, true));

            await publisher.Publish(EventType.ActivatedUser, userId);
            return userId;
        }
    }
}