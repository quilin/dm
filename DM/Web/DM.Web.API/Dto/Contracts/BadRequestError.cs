using System.Collections.Generic;
using System.Linq;
using DM.Services.Core.Exceptions;

namespace DM.Web.API.Dto.Contracts;

/// <summary>
/// Bad request error DTO
/// </summary>
public class BadRequestError : GeneralError
{
    /// <summary>
    /// Key-value pairs of invalid request properties
    /// </summary>
    public IDictionary<string, IEnumerable<string>> InvalidProperties { get; }

    /// <inheritdoc />
    public BadRequestError(string message, IDictionary<string, IEnumerable<string>> invalidProperties)
        : base(message) => InvalidProperties = invalidProperties;

    /// <inheritdoc />
    public BadRequestError(HttpBadRequestException exception)
        : base(exception.Message) => InvalidProperties = exception.ValidationErrors
        .ToDictionary(er => er.Key, er => Enumerable.Repeat(er.Value, 1));
}