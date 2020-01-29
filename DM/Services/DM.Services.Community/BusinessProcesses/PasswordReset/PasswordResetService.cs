using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.PasswordReset.Confirmation;
using DM.Services.Community.Dto;
using DM.Services.Core.Dto;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.PasswordReset
{
    /// <inheritdoc />
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IValidator<UserPasswordReset> validator;
        private readonly IPasswordResetTokenFactory tokenFactory;
        private readonly IPasswordResetRepository repository;
        private readonly IPasswordResetEmailSender emailSender;

        /// <inheritdoc />
        public PasswordResetService(
            IValidator<UserPasswordReset> validator,
            IPasswordResetTokenFactory tokenFactory,
            IPasswordResetRepository repository,
            IPasswordResetEmailSender emailSender)
        {
            this.validator = validator;
            this.tokenFactory = tokenFactory;
            this.repository = repository;
            this.emailSender = emailSender;
        }

        /// <inheritdoc />
        public async Task<GeneralUser> Reset(UserPasswordReset passwordReset)
        {
            await validator.ValidateAndThrowAsync(passwordReset);
            var user = await repository.FindUser(passwordReset.Login);

            var token = tokenFactory.Create(user.UserId);
            await repository.CreateToken(token);

            await emailSender.Send(user.Email, user.Login, token.TokenId);
            return user;
        }
    }
}