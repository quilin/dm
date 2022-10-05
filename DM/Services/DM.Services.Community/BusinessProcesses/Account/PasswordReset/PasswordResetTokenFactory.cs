using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Community.BusinessProcesses.Account.PasswordReset;

/// <inheritdoc />
internal class PasswordResetTokenFactory : IPasswordResetTokenFactory
{
    private readonly IGuidFactory guidFactory;
    private readonly IDateTimeProvider dateTimeProvider;

    /// <inheritdoc />
    public PasswordResetTokenFactory(
        IGuidFactory guidFactory,
        IDateTimeProvider dateTimeProvider)
    {
        this.guidFactory = guidFactory;
        this.dateTimeProvider = dateTimeProvider;
    }
        
    /// <inheritdoc />
    public Token Create(Guid userId)
    {
        return new Token
        {
            TokenId = guidFactory.Create(),
            UserId = userId,
            Type = TokenType.PasswordChange,
            CreateDate = dateTimeProvider.Now,
            IsRemoved = false
        };
    }
}