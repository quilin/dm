using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Account.Activation;
using DM.Services.Community.BusinessProcesses.Account.EmailChange.Confirmation;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Account.EmailChange;

/// <inheritdoc />
internal class EmailChangeService : IEmailChangeService
{
    private readonly IValidator<UserEmailChange> validator;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IActivationTokenFactory tokenFactory;
    private readonly IEmailChangeRepository repository;
    private readonly IEmailChangeMailSender mailSender;

    /// <inheritdoc />
    public EmailChangeService(
        IValidator<UserEmailChange> validator,
        IUpdateBuilderFactory updateBuilderFactory,
        IActivationTokenFactory tokenFactory,
        IEmailChangeRepository repository,
        IEmailChangeMailSender mailSender)
    {
        this.validator = validator;
        this.updateBuilderFactory = updateBuilderFactory;
        this.tokenFactory = tokenFactory;
        this.repository = repository;
        this.mailSender = mailSender;
    }

    /// <inheritdoc />
    public async Task<GeneralUser> Change(UserEmailChange emailChange)
    {
        await validator.ValidateAndThrowAsync(emailChange);
        var user = await repository.FindUser(emailChange.Login);

        var updateUser = updateBuilderFactory.Create<User>(user.UserId)
            .Field(u => u.Email, emailChange.Email)
            .Field(u => u.Activated, false);
        var token = tokenFactory.Create(user.UserId);

        await repository.Update(updateUser, token);
        await mailSender.Send(emailChange.Email, emailChange.Login, token.TokenId);

        return user;
    }
}