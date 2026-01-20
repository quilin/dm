using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;

namespace DM.Services.Community.BusinessProcesses.Account.Verification;

/// <inheritdoc />
internal class TokenVerificationService(
    ITokenVerificationRepository repository) : ITokenVerificationService
{
    /// <inheritdoc />
    public async Task<GeneralUser> Verify(Guid token, CancellationToken cancellationToken)
    {
        var owner = await repository.GetTokenOwner(token, cancellationToken);
        if (owner == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Token is invalid");
        }

        return owner;
    }
}