using System;
using System.Linq;
using System.Net;
using DM.Web.Classic.Controllers.ActionResults;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers
{
    public class DmControllerBase : Controller
    {
        protected IActionResult AjaxError(HttpStatusCode statusCode, string message, object data = null)
        {
            return new AjaxErrorResult(statusCode, message, data);
        }

        protected IActionResult AjaxFormError<T>(params T[] errors)
            where T : struct
        {
            var fieldErrors = errors.ToDictionary(
                e => e.GetAttribute<T, FormFieldLinkAttribute>().FieldName,
                e => ((Enum) (object) e).GetDescription());
            return new AjaxErrorResult(HttpStatusCode.BadRequest, null, fieldErrors);
        }
    }
}