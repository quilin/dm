using System.Threading;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Account.Activation;
using DM.Services.Community.BusinessProcesses.Account.EmailChange.Confirmation;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Account.EmailChange;

/// <inheritdoc />
internal class EmailChangeService(
    IValidator<UserEmailChange> validator,
    IUpdateBuilderFactory updateBuilderFactory,
    IActivationTokenFactory tokenFactory,
    IEmailChangeRepository repository,
    IEmailChangeMailSender mailSender) : IEmailChangeService
{
    /// <inheritdoc />
    public async Task<GeneralUser> Change(UserEmailChange emailChange, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(emailChange, cancellationToken);
        var user = await repository.FindUser(emailChange.Login, cancellationToken);

        var updateUser = updateBuilderFactory.Create<User>(user.UserId)
            .Field(u => u.Email, emailChange.Email)
            .Field(u => u.Activated, false);
        var token = tokenFactory.Create(user.UserId);

        await repository.Update(updateUser, token, cancellationToken);
        await mailSender.Send(emailChange.Email, emailChange.Login, token.TokenId, cancellationToken);

        return user;
    }
}