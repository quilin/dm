using System;

namespace Migration.BusinessObjects
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastVisitDate { get; set; }
        public int Role { get; set; }
        public string Salt { get; set; }
        public string PasswordHash { get; set; }
        public int QualityRating { get; set; }
        public int QuantityRating { get; set; }
        public bool Activated { get; set; }
        public string TimezoneId { get; set; }
        public bool RatingDisabled { get; set; }
        public int AccessPolicy { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool CanMerge { get; set; }
        public Guid? MergeRequested { get; set; }
    }
}