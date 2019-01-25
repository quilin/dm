using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;

namespace DM.Services.DataAccess.BusinessObjects.Users
{
    [Table("Users")]
    public class User : IPublicUser
    {
        [Key]
        public Guid UserId { get; set; }

        public string Login { get; set; }
        public string Email { get; set; }

        public string ProfilePictureUrl { get; set; }

        public DateTime RegistrationDate { get; set; }
        public DateTime? LastVisitDate { get; set; }
        public string TimezoneId { get; set; }

        public UserRole Role { get; set; }
        public AccessPolicy AccessPolicy { get; set; }

        public string Salt { get; set; }
        public string PasswordHash { get; set; }

        public bool RatingDisabled { get; set; }
        public int QualityRating { get; set; }
        public int QuantityRating { get; set; }

        public bool Activated { get; set; }
        public bool CanMerge { get; set; }
        public Guid? MergeRequested { get; set; }

        [InverseProperty(nameof(ForumTopic.Author))]
        public ICollection<ForumTopic> Topics { get; set; }

        [InverseProperty(nameof(Comment.Author))]
        public ICollection<Comment> Comments { get; set; }
    }
}