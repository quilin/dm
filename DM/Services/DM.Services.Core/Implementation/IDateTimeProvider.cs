using System;

namespace DM.Services.Core.Implementation
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}