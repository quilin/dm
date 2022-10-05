using System;

namespace DM.Services.Core.Implementation.CorrelationToken;

/// <summary>
/// Correlation token setter
/// </summary>
public interface ICorrelationTokenSetter
{
    /// <summary>
    /// Save correlation token for current context
    /// </summary>
    Guid Current { set; }
}