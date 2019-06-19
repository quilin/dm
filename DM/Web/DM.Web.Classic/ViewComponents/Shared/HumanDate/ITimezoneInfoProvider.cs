using System;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Classic.ViewComponents.Shared.HumanDate
{
    public interface ITimezoneInfoProvider
    {
        void Set(string timezoneId, HttpContext httpContext);
        TimeSpan Delta { get; }
        TimeZoneInfo Current { get; }
    }
}