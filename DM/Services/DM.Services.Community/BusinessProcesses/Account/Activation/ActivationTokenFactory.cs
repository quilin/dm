using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Community.BusinessProcesses.Account.Activation;

/// <inheritdoc />
internal class ActivationTokenFactory : IActivationTokenFactory
{
    private readonly IGuidFactory guidFactory;
    private readonly IDateTimeProvider dateTimeProvider;

    /// <inheritdoc />
    public ActivationTokenFactory(
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
            Type = TokenType.Activation,
            CreateDate = dateTimeProvider.Now,
            IsRemoved = false
        };
    }
}