using System;
using System.Globalization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DM.Web.Classic.Extensions.HumanDate
{
    public static class HumanDateExtension
    {
        public static IHtmlContent HumanDate(this DateTime dateTime, ITimezoneInfoProvider provider, bool withTooltip)
        {
            var humanDate = dateTime.HumanFormat(provider);
            var formatDate = dateTime.Format(provider, true);

            if (formatDate == humanDate)
            {
                return new HtmlContentBuilder().AppendHtml(formatDate);
            }

            if (!withTooltip)
            {
                return new HtmlContentBuilder().AppendHtml(humanDate);
            }

            var formatDateTag = new TagBuilder("span") {TagRenderMode = TagRenderMode.Normal};
            formatDateTag.AddCssClass("human-date-tip");
            formatDateTag.InnerHtml.AppendHtml(formatDate);

            var wrapperTag = new TagBuilder("span") {TagRenderMode = TagRenderMode.Normal};
            wrapperTag.AddCssClass("human-date-wrapper");
            wrapperTag.InnerHtml.Append(humanDate);
            wrapperTag.InnerHtml.AppendHtml(formatDateTag);

            return wrapperTag;
        }

        public static IHtmlContent HumanDate(this DateTime? dateTime, ITimezoneInfoProvider provider, string emptyValue)
        {
            return dateTime.HasValue
                ? HumanDate(dateTime.Value, provider, true)
                : new HtmlString(emptyValue);
        }

        private static string HumanFormat(this DateTime dateTime, ITimezoneInfoProvider provider)
        {
            return dateTime.Humanize(DateTime.UtcNow, provider.Delta);
        }

        public static string FormatForChat(this DateTime dateTime, ITimezoneInfoProvider provider)
        {
            return dateTime.Add(provider.Delta).ToString("dd.MM HH:mm", CultureInfo.InvariantCulture);
        }

        private static string Humanize(this DateTime dateTime, DateTime currentDateTime, TimeSpan timezoneOffset)
        {
            var offset = currentDateTime - dateTime;

            return offset.TotalSeconds > 0
                ? PastHumanize(dateTime, currentDateTime, timezoneOffset, offset)
                : FutureHumanize(offset);
        }

        private static string FutureHumanize(TimeSpan offset)
        {
            offset = -offset;

            if (offset.TotalDays > 1000)
            {
                return "очень нескоро";
            }

            if (offset.TotalDays > 1)
            {
                var days = (int) offset.TotalDays;
                var plural = days > 10 && days < 20 || days % 10 > 4 || days % 10 == 0
                    ? "дней"
                    : days % 10 <= 4
                        ? "дня"
                        : "день";
                return $"через {days} {plural}";
            }

            if (offset.TotalHours > 1)
            {
                var hours = (int) offset.TotalHours;
                var plural = hours > 10 && hours < 20 || hours % 10 > 4 || hours % 10 == 0
                    ? "часов"
                    : hours % 10 <= 4
                        ? "часа"
                        : "час";
                return $"через {hours} {plural}";
            }

            return "меньше чем через час";
        }

        private static string PastHumanize(DateTime dateTime, DateTime currentDateTime, TimeSpan timezoneOffset,
            TimeSpan offset)
        {
            if (offset.TotalSeconds < 30)
            {
                return "только что";
            }

            if (offset.TotalMinutes < 1)
            {
                return "меньше минуты назад";
            }

            if (offset.TotalHours < 1)
            {
                var minutes = (int) Math.Floor(offset.TotalMinutes);
                var minutesFirstDivision = minutes % 10;
                var pluralSuffix = minutes > 10 && minutes < 20 ||
                                   minutesFirstDivision > 4 || minutesFirstDivision == 0
                    ? string.Empty
                    : (minutesFirstDivision == 1 ? "у" : "ы");
                return minutes == 1 ? "минуту назад" : $"{minutes} минут{pluralSuffix} назад";
            }

            var currentDateTimeInTimezone = currentDateTime + timezoneOffset;
            var currentMidnightInTimezone =
                new DateTime(currentDateTimeInTimezone.Year, currentDateTime.Month, currentDateTime.Day);
            var currentMidnightOffset = currentDateTimeInTimezone - currentMidnightInTimezone;

            var dateTimeInTimezone = dateTime + timezoneOffset;

            if (offset.TotalHours <= currentMidnightOffset.TotalHours)
            {
                return $"{dateTimeInTimezone:HH:mm}";
            }

            return offset.TotalHours <= currentMidnightOffset.TotalHours + 24
                ? $"вчера в {dateTimeInTimezone:HH:mm}"
                : $"{dateTimeInTimezone:dd.MM.yyyy HH:mm}";
        }
    }
}