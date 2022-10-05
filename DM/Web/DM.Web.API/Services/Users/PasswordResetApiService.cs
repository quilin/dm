using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Account.PasswordChange;
using DM.Services.Community.BusinessProcesses.Account.PasswordReset;
using DM.Services.Mail.Sender;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users;

/// <inheritdoc />
internal class PasswordResetApiService : IPasswordResetApiService
{
    private readonly IPasswordResetService passwordResetService;
    private readonly IPasswordChangeService passwordChangeService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public PasswordResetApiService(
        IPasswordResetService passwordResetService,
        IPasswordChangeService passwordChangeService,
        IMapper mapper)
    {
        this.passwordResetService = passwordResetService;
        this.passwordChangeService = passwordChangeService;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Envelope<User>> Reset(ResetPassword resetPassword)
    {
        var userPasswordReset = mapper.Map<UserPasswordReset>(resetPassword);
        var user = await passwordResetService.Reset(userPasswordReset);
        return new Envelope<User>(mapper.Map<User>(user), new {email = user.Email.Obfuscate()});
    }

    /// <inheritdoc />
    public async Task<Envelope<User>> Change(ChangePassword changePassword)
    {
        var userPasswordChange = mapper.Map<UserPasswordChange>(changePassword);
        var user = await passwordChangeService.Change(userPasswordChange);
        return new Envelope<User>(mapper.Map<User>(user));
    }
}