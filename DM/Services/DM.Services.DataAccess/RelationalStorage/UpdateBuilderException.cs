using System;

namespace DM.Services.DataAccess.RelationalStorage;

/// <inheritdoc />
internal class UpdateBuilderException : Exception
{
    /// <inheritdoc />
    public UpdateBuilderException(string message) : base(message)
    {
    }
}