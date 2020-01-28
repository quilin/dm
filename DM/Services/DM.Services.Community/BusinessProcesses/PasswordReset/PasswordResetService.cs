using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Authentication.Repositories;
using DM.Services.Community.BusinessProcesses.PasswordReset.Confirmation;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;

namespace DM.Services.Community.BusinessProcesses.PasswordReset
{
    /// <inheritdoc />
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IAuthenticationRepository authenticationRepository;
        private readonly IPasswordResetTokenFactory tokenFactory;
        private readonly IPasswordResetRepository repository;
        private readonly IPasswordResetEmailSender emailSender;

        /// <inheritdoc />
        public PasswordResetService(
            IAuthenticationRepository authenticationRepository,
            IPasswordResetTokenFactory tokenFactory,
            IPasswordResetRepository repository,
            IPasswordResetEmailSender emailSender)
        {
            this.authenticationRepository = authenticationRepository;
            this.tokenFactory = tokenFactory;
            this.repository = repository;
            this.emailSender = emailSender;
        }
        
        /// <inheritdoc />
        public async Task<GeneralUser> Reset(string login, string email)
        {
            var (success, user) = await authenticationRepository.TryFindUser(login);
            if (!success)
            {
                throw new HttpBadRequestException(new Dictionary<string, string>
                {
                    [nameof(login)] = "User not found"
                });
            }

            if (!user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new HttpBadRequestException(new Dictionary<string, string>
                {
                    [nameof(email)] = "User email doesn't match"
                });
            }

            var token = tokenFactory.Create(user.UserId);
            await repository.CreateToken(token);

            await emailSender.Send(user.Email, user.Login, token.TokenId);
            return user;
        }
    }
}