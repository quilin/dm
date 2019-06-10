using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Implementation;
using DM.Services.Core.Exceptions;
using DM.Services.Core.Implementation.CorrelationToken;
using DM.Web.API.Dto.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DM.Web.API.Middleware
{
    /// <summary>
    /// Middleware for exceptions handling
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        /// <inheritdoc />
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Before request
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="logger">Logger</param>
        /// <param name="identitySetter">Identity setter for Serilog issue fix</param>
        /// <param name="correlationTokenProvider">Correlation token for support assistance</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext,
            ILogger<ErrorHandlingMiddleware> logger,
            IIdentitySetter identitySetter,
            ICorrelationTokenProvider correlationTokenProvider)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception e)
            {
                identitySetter.Refresh();
                int statusCode;
                GeneralError error;
                switch (e)
                {
                    case HttpBadRequestException badRequestException:
                        statusCode = (int) badRequestException.StatusCode;
                        error = new BadRequestError(badRequestException);
                        break;
                    case IntentionManagerException securityException:
                        statusCode = (int) securityException.StatusCode;
                        error = new GeneralError(securityException.Message);
                        logger.LogWarning(securityException, securityException.Message);
                        break;
                    case HttpException httpException:
                        statusCode = (int) httpException.StatusCode;
                        error = new GeneralError(httpException.Message);
                        break;
                    case NotImplementedException notImplementedException:
                        statusCode = (int) HttpStatusCode.NotImplemented;
                        error = new GeneralError(notImplementedException.Message);
                        break;
                    case ValidationException validationException:
                        statusCode = (int) HttpStatusCode.BadRequest;
                        error = new BadRequestError("Validation failed",
                            validationException.Errors.ToDictionary(er => er.PropertyName, er => er.ErrorMessage));
                        break;
                    default:
                        statusCode = (int) HttpStatusCode.InternalServerError;
                        error = new GeneralError($"Server error. Address the administration for technical support. Use the following token to help us identify your issue: {correlationTokenProvider.Current}");
                        logger.LogCritical(e, e.Message);
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