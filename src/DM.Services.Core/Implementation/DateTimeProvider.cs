using System;

namespace DM.Services.Core.Implementation;

/// <inheritdoc />
internal class DateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc />
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}