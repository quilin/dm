using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DM.Web.Classic.Controllers.ActionResults
{
    public class AjaxErrorResult : IActionResult
    {
        private readonly HttpStatusCode httpCode;
        private readonly string message;
        private readonly object obj;

        public AjaxErrorResult(HttpStatusCode httpCode, string message, object obj)
        {
            this.httpCode = httpCode;
            this.message = message;
            this.obj = obj;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int) httpCode;
            return response.WriteAsync(JsonConvert.SerializeObject(GetErrorObject()));
        }

        private object GetErrorObject()
        {
            return string.IsNullOrEmpty(message)
                       ? (object) new {obj}
                       : new {message, obj};
        }
    }
}