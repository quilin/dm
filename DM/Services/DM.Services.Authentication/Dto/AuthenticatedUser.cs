using System.Collections.Generic;
using System.Linq;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Authentication.Dto
{
    public class AuthenticatedUser : GeneralUser
    {
        public bool Activated { get; set; }
        public string Salt { get; set; }
        public string PasswordHash { get; set; }
        public bool IsRemoved { get; set; }
        public IEnumerable<AccessPolicy> AccessRestrictionPolicies { get; set; }

        public AccessPolicy GeneralAccessPolicy =>
            AccessRestrictionPolicies.Aggregate(AccessPolicy, (seed, restriction) => seed | restriction);

        public static readonly AuthenticatedUser Guest = new AuthenticatedUser();
    }
}