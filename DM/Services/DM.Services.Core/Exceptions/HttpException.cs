using System;
using System.Collections.Generic;
using System.Net;

namespace DM.Services.Core.Exceptions
{
    public class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public HttpException(HttpStatusCode statusCode, string message = null)
            : base(message) => StatusCode = statusCode;
    }

    public class HttpBadRequestException : HttpException
    {
        public IDictionary<string, string> ValidationErrors { get; }

        public HttpBadRequestException(IDictionary<string, string> errors, string message)
            : base(HttpStatusCode.BadRequest, message) => ValidationErrors = errors;
    }
}