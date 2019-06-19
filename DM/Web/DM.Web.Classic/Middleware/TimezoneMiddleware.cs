using System.Threading.Tasks;
using DM.Web.Classic.ViewComponents.Shared.HumanDate;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Classic.Middleware
{
    public class TimezoneMiddleware
    {
        private readonly RequestDelegate next;

        public TimezoneMiddleware(
            RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext,
            ITimezoneInfoProvider timezoneInfoProvider)
        {
            await next(httpContext);
        }
    }
}