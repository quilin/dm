using System;
using System.Net;
using System.Text;
using System.Text.Json;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;

namespace DM.Services.Common.Authorization;

/// <summary>
/// Specific exception when user tries to perform unauthorized action
/// </summary>
public class IntentionManagerException : HttpException
{
    /// <inheritdoc />
    public IntentionManagerException(IUser user, Enum intention, object target)
        : base(HttpStatusCode.Forbidden, GenerateMessage(user, intention, target))
    {
    }

    /// <inheritdoc />
    public IntentionManagerException(IUser user, Enum intention)
        : base(HttpStatusCode.Forbidden, GenerateMessage(user, intention))
    {
    }

    private static string GenerateMessage(IUser user, Enum intention, object target = default)
    {
        var result = new StringBuilder($"User is not allowed to perform {intention} action");
        if (target == null)
        {
            return result.ToString();
        }

        result.Append($" with {JsonSerializer.Serialize(target)}");
        return result.ToString();
    }
}