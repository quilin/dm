using System;

namespace DM.Services.Core.Implementation.CorrelationToken;

/// <summary>
/// Correlation token provider
/// </summary>
public interface ICorrelationTokenProvider
{
    /// <summary>
    /// Get current context correlation token
    /// </summary>
    Guid Current { get; }
}