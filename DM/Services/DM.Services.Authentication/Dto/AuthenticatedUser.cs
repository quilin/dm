using System;
using System.Collections.Generic;
using System.Linq;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Authentication.Dto
{
    public class AuthenticatedUser : IUser
    {
        public Guid UserId { get; set; }
        public string Login { get; set; }
        public string ProfilePictureUrl { get; set; }
        public UserRole Role { get; set; }
        public AccessPolicy AccessPolicy { get; set; }
        public DateTime? LastVisitDate { get; set; }
        public bool RatingDisabled { get; set; }
        public int QualityRating { get; set; }
        public int QuantityRating { get; set; }
        public bool Activated { get; set; }
        public string Salt { get; set; }
        public string PasswordHash { get; set; }
        public bool IsRemoved { get; set; }
        public IEnumerable<AccessPolicy> AccessRestrictionPolicies { get; set; }

        public AccessPolicy GeneralAccessPolicy =>
            AccessRestrictionPolicies.Aggregate(AccessPolicy, (seed, restriction) => seed | restriction);

        public static readonly AuthenticatedUser Guest = new AuthenticatedUser();

        public bool IsGuest => Role == UserRole.Guest;
    }
}