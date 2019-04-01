using System;

namespace DM.Services.Authentication.Implementation.CorrelationToken
{
    public interface ICorrelationTokenProvider
    {
        Guid Current { get; }
    }
}