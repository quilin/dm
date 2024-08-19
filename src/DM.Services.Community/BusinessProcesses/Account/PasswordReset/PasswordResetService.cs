using System.Threading;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Account.PasswordReset.Confirmation;
using DM.Services.Community.BusinessProcesses.Users.Reading;
using DM.Services.Core.Dto;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Account.PasswordReset;

/// <inheritdoc />
internal class PasswordResetService(
    IValidator<UserPasswordReset> validator,
    IPasswordResetTokenFactory tokenFactory,
    IUserReadingRepository userReadingRepository,
    IPasswordResetRepository repository,
    IPasswordResetEmailSender emailSender) : IPasswordResetService
{
    /// <inheritdoc />
    public async Task<GeneralUser> Reset(UserPasswordReset passwordReset, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(passwordReset, cancellationToken);
        var user = await userReadingRepository.GetUserDetails(passwordReset.Login, cancellationToken);

        var token = tokenFactory.Create(user.UserId);
        await repository.CreateToken(token, cancellationToken);

        await emailSender.Send(user.Email, user.Login, token.TokenId, cancellationToken);
        return user;
    }
}