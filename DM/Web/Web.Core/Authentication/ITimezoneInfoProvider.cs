using System;
using Microsoft.AspNetCore.Http;

namespace Web.Core.Authentication
{
    public interface ITimezoneInfoProvider
    {
        void Set(string timezoneId, HttpContext httpContext);
        TimeSpan Delta { get; }
        TimeZoneInfo Current { get; }
    }
}