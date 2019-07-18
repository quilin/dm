using System;
using System.Threading.Tasks;
using DM.Web.Classic.Extensions.HumanDate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace DM.Web.Classic.ViewComponents.Shared.HumanDate
{
    public class HumanizedDate : ViewComponent
    {
        private readonly ITimezoneInfoProvider timezoneInfoProvider;

        public HumanizedDate(
            ITimezoneInfoProvider timezoneInfoProvider)
        {
            this.timezoneInfoProvider = timezoneInfoProvider;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(DateTimeOffset? date, bool? withTooltip, string emptyValue)
        {
            return await Task.Run(() => string.IsNullOrEmpty(emptyValue) && date.HasValue
                ? new HtmlContentViewComponentResult(date.Value.HumanDate(timezoneInfoProvider, withTooltip ?? true))
                : new HtmlContentViewComponentResult(date.HumanDate(timezoneInfoProvider, emptyValue)));
        }
    }
}