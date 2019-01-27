using System;
using System.Globalization;
using Web.Core.Authentication;

namespace Web.Core.Extensions.DateTimeExtensions
{
    public static class DateFormatExtensions
    {
        public static string Format(this DateTime dateTime, ITimezoneInfoProvider provider, bool withTime)
        {
            var format = withTime
                ? "dd.MM.yyyy HH:mm"
                : "dd.MM.yyyy";

            return dateTime.Add(provider.Delta).ToString(format, CultureInfo.InvariantCulture);
        }
    }
}