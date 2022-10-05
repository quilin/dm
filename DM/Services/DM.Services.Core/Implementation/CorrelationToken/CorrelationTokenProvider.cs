using System;
using Serilog.Context;

namespace DM.Services.Core.Implementation.CorrelationToken;

/// <summary>
/// Correlation token storage
/// </summary>
internal class CorrelationTokenProvider : ICorrelationTokenProvider, ICorrelationTokenSetter
{
    private Lazy<Guid> token;

    /// <param name="guidFactory"></param>
    /// <inheritdoc />
    public CorrelationTokenProvider(IGuidFactory guidFactory)
    {
        token = new Lazy<Guid>(guidFactory.Create);
    }
        
    /// <inheritdoc cref="ICorrelationTokenProvider" />
    public Guid Current
    {
        get => token.Value;
        set
        {
            if (token.IsValueCreated)
            {
                return;
            }

            LogContext.PushProperty("CorrelationToken", value);
            token = new Lazy<Guid>(() => value);
        }
    }
}