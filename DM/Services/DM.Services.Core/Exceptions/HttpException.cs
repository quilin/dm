using System;
using System.Collections.Generic;
using System.Net;

namespace DM.Services.Core.Exceptions
{
    /// <summary>
    /// General HTTP exception
    /// </summary>
    public class HttpException : Exception
    {
        /// <summary>
        /// HTTP status code
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        public HttpException(HttpStatusCode statusCode, string message) : base(message) => StatusCode = statusCode;
    }

    /// <summary>
    /// General bad request HTTP exception
    /// </summary>
    public class HttpBadRequestException : HttpException
    {
        /// <summary>
        /// Key-value list of invalid fields and validation errors
        /// </summary>
        public IDictionary<string, string> ValidationErrors { get; }

        public HttpBadRequestException(IDictionary<string, string> errors,
            string message = "Invalid request parameters")
            : base(HttpStatusCode.BadRequest, message) => ValidationErrors = errors;
    }
}