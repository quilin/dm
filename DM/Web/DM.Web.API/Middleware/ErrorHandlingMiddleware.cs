using System;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DM.Services.Core.Exceptions;
using DM.Web.API.Dto.Contracts;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DM.Web.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception e)
            {
                int statusCode;
                GeneralError error;
                switch (e)
                {
                    case HttpBadRequestException badRequestException:
                        statusCode = (int) badRequestException.StatusCode;
                        error = new BadRequestError(badRequestException.Message, badRequestException.ValidationErrors);
                        break;
                    case HttpException httpException:
                        statusCode = (int) httpException.StatusCode;
                        error = new GeneralError(httpException.Message);
                        break;
                    case NotImplementedException notImplementedException:
                        statusCode = (int) HttpStatusCode.NotImplemented;
                        error = new GeneralError(notImplementedException.Message);
                        break;
                    default:
                        statusCode = (int) HttpStatusCode.InternalServerError;
                        error = new GeneralError("Server error. Address the administration for technical support.");
                        break;
                }

                httpContext.Response.StatusCode = statusCode;
                httpContext.Response.ContentType = MediaTypeNames.Application.Json;
                var errorData = JsonConvert.SerializeObject(new {error}, Formatting.None, serializerSettings);
                await httpContext.Response.WriteAsync(errorData, Encoding.UTF8);
            }
        }
    }
}