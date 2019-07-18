using System;
using System.Threading.Tasks;
using DM.Web.Classic.Extensions.HumanDate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace DM.Web.Classic.ViewComponents.Shared.HumanDate
{
    public class DateFormat : ViewComponent
    {
        private readonly ITimezoneInfoProvider timezoneInfoProvider;

        public DateFormat(
            ITimezoneInfoProvider timezoneInfoProvider)
        {
            this.timezoneInfoProvider = timezoneInfoProvider;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(DateTimeOffset date, bool? withTime)
        {
            return await Task.Run(() =>
                new ContentViewComponentResult(date.Format(timezoneInfoProvider, withTime ?? false)));
        }
    }
}