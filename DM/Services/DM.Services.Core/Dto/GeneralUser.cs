using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Core.Dto
{
    public class GeneralUser : IUser
    {
        public Guid UserId { get; set; }
        public string Login { get; set; }
        public UserRole Role { get; set; }
        public AccessPolicy AccessPolicy { get; set; }
        public DateTime? LastVisitDate { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool RatingDisabled { get; set; }
        public int QualityRating { get; set; }
        public int QuantityRating { get; set; }
    }
}