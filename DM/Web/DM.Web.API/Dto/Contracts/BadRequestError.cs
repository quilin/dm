using System.Collections.Generic;

namespace DM.Web.API.Dto.Contracts
{
    public class BadRequestError : GeneralError
    {
        public IDictionary<string, string> InvalidProperties { get; }

        public BadRequestError(string message, IDictionary<string, string> invalidProperties)
            : base(message) => InvalidProperties = invalidProperties;
    }
}