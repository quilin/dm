using System.Linq;
using System.Net;
using DM.Services.Core.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DM.Web.Classic.Middleware
{
    public class ValidationRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!filterContext.ModelState.IsValid)
            {
                var errors = filterContext.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                
                throw new HttpException(HttpStatusCode.BadRequest,
                    $"Wrong parameters. ModelState errors: {string.Join("; ", errors)}");
            }
        }
    }
}