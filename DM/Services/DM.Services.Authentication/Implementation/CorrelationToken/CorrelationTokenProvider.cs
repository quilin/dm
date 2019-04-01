using System;

namespace DM.Services.Authentication.Implementation.CorrelationToken
{
    public class CorrelationTokenProvider : ICorrelationTokenProvider, ICorrelationTokenSetter
    {
        private Lazy<Guid> token = new Lazy<Guid>(Guid.NewGuid);

        public Guid Current
        {
            get => token.Value;
            set => token = new Lazy<Guid>(() => value);
        }
    }
}