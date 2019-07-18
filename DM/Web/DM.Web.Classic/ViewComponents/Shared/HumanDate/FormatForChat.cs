using System;
using System.Threading.Tasks;
using DM.Web.Classic.Extensions.HumanDate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace DM.Web.Classic.ViewComponents.Shared.HumanDate
{
    public class FormatForChat : ViewComponent
    {
        private readonly ITimezoneInfoProvider timezoneInfoProvider;

        public FormatForChat(
            ITimezoneInfoProvider timezoneInfoProvider)
        {
            this.timezoneInfoProvider = timezoneInfoProvider;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(DateTimeOffset date)
        {
            return await Task.Run(() => new ContentViewComponentResult(date.FormatForChat(timezoneInfoProvider)));
        }
    }
}