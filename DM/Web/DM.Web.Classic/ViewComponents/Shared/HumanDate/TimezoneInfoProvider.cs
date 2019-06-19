using System;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Classic.ViewComponents.Shared.HumanDate
{
    public class TimezoneInfoProvider : ITimezoneInfoProvider
    {
        private const string TimezoneOffsetCookieName = "__Timezone__Offset__";
        private string TimezoneId { get; set; }
        private int? OffsetFromCookie { get; set; }
        
        public void Set(string timezoneId, HttpContext httpContext)
        {
            TimezoneId = timezoneId;
            OffsetFromCookie = httpContext.Request.Cookies.TryGetValue(TimezoneOffsetCookieName, out var offsetValue)
                ? int.TryParse(offsetValue, out var offset)
                    ? offset
                    : (int?) null
                : null;
        }
        
        public TimeSpan Delta
        {
            get
            {
                var timeZoneInfo = Current;
                if (Current != null)
                {
                    return timeZoneInfo.GetUtcOffset(DateTime.UtcNow);
                }

                return OffsetFromCookie.HasValue
                    ? new TimeSpan(0, -OffsetFromCookie.Value, 0)
                    : TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
            }
        }
        
        public TimeZoneInfo Current
        {
            get
            {
                if (string.IsNullOrEmpty(TimezoneId))
                {
                    return null;
                }
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById(TimezoneId);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}