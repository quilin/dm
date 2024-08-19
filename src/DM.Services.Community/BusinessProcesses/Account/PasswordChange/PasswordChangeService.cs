using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Account.PasswordChange;

/// <inheritdoc />
internal class PasswordChangeService(
    IValidator<UserPasswordChange> validator,
    ISecurityManager securityManager,
    IUpdateBuilderFactory updateBuilderFactory,
    IPasswordChangeRepository repository,
    IAuthenticationService authenticationService,
    IIdentityProvider identityProvider) : IPasswordChangeService
{
    /// <inheritdoc />
    public async Task<GeneralUser> Change(UserPasswordChange passwordChange, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(passwordChange, cancellationToken);
        var user = passwordChange.Token.HasValue
            ? await repository.FindUser(passwordChange.Token.Value, cancellationToken)
            : identityProvider.Current.User;

        var (hash, salt) = securityManager.GeneratePassword(passwordChange.NewPassword);
        var userUpdate = updateBuilderFactory.Create<User>(user.UserId)
            .Field(u => u.PasswordHash, hash)
            .Field(u => u.Salt, salt);
        var tokenUpdate = passwordChange.Token.HasValue
            ? updateBuilderFactory.Create<Token>(passwordChange.Token.Value)
                .Field(t => t.IsRemoved, true)
            : null;

        await repository.UpdatePassword(userUpdate, tokenUpdate, cancellationToken);
        await authenticationService.LogoutElsewhere(cancellationToken);

        return user;
    }
}