using System;
using Serilog.Context;

namespace DM.Services.Core.Implementation.CorrelationToken
{
    /// <summary>
    /// Correlation token storage
    /// </summary>
    public class CorrelationTokenProvider : ICorrelationTokenProvider, ICorrelationTokenSetter
    {
        private Lazy<Guid> token = new Lazy<Guid>(Guid.NewGuid);

        /// <inheritdoc cref="ICorrelationTokenProvider" />
        public Guid Current
        {
            get => token.Value;
            set
            {
                token = new Lazy<Guid>(() => value);
                LogContext.PushProperty("CorrelationToken", value);
            }
        }
    }
}