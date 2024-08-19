using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Account.PasswordChange;
using DM.Services.Community.BusinessProcesses.Account.PasswordReset;
using DM.Services.Mail.Sender;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users;

/// <inheritdoc />
internal class PasswordResetApiService(
    IPasswordResetService passwordResetService,
    IPasswordChangeService passwordChangeService,
    IMapper mapper) : IPasswordResetApiService
{
    /// <inheritdoc />
    public async Task<Envelope<User>> Reset(ResetPassword resetPassword, CancellationToken cancellationToken)
    {
        var userPasswordReset = mapper.Map<UserPasswordReset>(resetPassword);
        var user = await passwordResetService.Reset(userPasswordReset, cancellationToken);
        return new Envelope<User>(mapper.Map<User>(user), new {email = user.Email.Obfuscate()});
    }

    /// <inheritdoc />
    public async Task<Envelope<User>> Change(ChangePassword changePassword, CancellationToken cancellationToken)
    {
        var userPasswordChange = mapper.Map<UserPasswordChange>(changePassword);
        var user = await passwordChangeService.Change(userPasswordChange, cancellationToken);
        return new Envelope<User>(mapper.Map<User>(user));
    }
}