using System;

namespace DM.Services.Authentication.Implementation.CorrelationToken
{
    public interface ICorrelationTokenSetter
    {
        Guid Current { set; }
    }
}