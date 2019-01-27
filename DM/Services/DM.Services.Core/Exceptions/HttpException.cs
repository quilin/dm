using System;
using System.Net;

namespace DM.Services.Core.Exceptions
{
    public class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public HttpException(HttpStatusCode statusCode, string message)
            : base(message) => StatusCode = statusCode;
    }
}