using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Account.PasswordReset.Confirmation;
using DM.Services.Community.BusinessProcesses.Users.Reading;
using DM.Services.Core.Dto;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Account.PasswordReset;

/// <inheritdoc />
internal class PasswordResetService : IPasswordResetService
{
    private readonly IValidator<UserPasswordReset> validator;
    private readonly IPasswordResetTokenFactory tokenFactory;
    private readonly IUserReadingRepository userReadingRepository;
    private readonly IPasswordResetRepository repository;
    private readonly IPasswordResetEmailSender emailSender;

    /// <inheritdoc />
    public PasswordResetService(
        IValidator<UserPasswordReset> validator,
        IPasswordResetTokenFactory tokenFactory,
        IUserReadingRepository userReadingRepository,
        IPasswordResetRepository repository,
        IPasswordResetEmailSender emailSender)
    {
        this.validator = validator;
        this.tokenFactory = tokenFactory;
        this.userReadingRepository = userReadingRepository;
        this.repository = repository;
        this.emailSender = emailSender;
    }

    /// <inheritdoc />
    public async Task<GeneralUser> Reset(UserPasswordReset passwordReset)
    {
        await validator.ValidateAndThrowAsync(passwordReset);
        var user = await userReadingRepository.GetUserDetails(passwordReset.Login);

        var token = tokenFactory.Create(user.UserId);
        await repository.CreateToken(token);

        await emailSender.Send(user.Email, user.Login, token.TokenId);
        return user;
    }
}