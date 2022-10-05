using System;

namespace DM.Services.Core.Implementation;

/// <summary>
/// Provides date in the same format across the application
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Current moment in UTC
    /// </summary>
    DateTimeOffset Now { get; }
}