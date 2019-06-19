using System;
using System.Globalization;
using DM.Web.Classic.ViewComponents.Shared.HumanDate;

namespace DM.Web.Classic.Extensions.HumanDate
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