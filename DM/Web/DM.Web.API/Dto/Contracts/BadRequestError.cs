using System.Collections.Generic;

namespace DM.Web.API.Dto.Contracts
{
    public class BadRequestError : GeneralError
    {
        public IDictionary<string, string> InvalidProperties { get; set; }
    }
}