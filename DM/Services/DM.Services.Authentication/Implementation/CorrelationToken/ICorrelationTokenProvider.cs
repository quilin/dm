using System;

namespace DM.Services.Authentication.Implementation.CorrelationToken
{
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
}