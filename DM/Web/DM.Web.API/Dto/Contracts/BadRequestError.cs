using System.Collections.Generic;
using DM.Services.Core.Exceptions;

namespace DM.Web.API.Dto.Contracts
{
    /// <summary>
    /// Bad request error DTO
    /// </summary>
    public class BadRequestError : GeneralError
    {
        /// <summary>
        /// Key-value pairs of invalid request properties
        /// </summary>
        public IDictionary<string, string> InvalidProperties { get; }

        /// <inheritdoc />
        public BadRequestError(string message, IDictionary<string, string> invalidProperties)
            : base(message) => InvalidProperties = invalidProperties;

        /// <inheritdoc />
        public BadRequestError(HttpBadRequestException exception)
            : base(exception.Message) => InvalidProperties = exception.ValidationErrors;
    }
}