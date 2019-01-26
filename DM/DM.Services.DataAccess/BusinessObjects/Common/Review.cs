using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Common
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        public Guid ReviewId { get; set; }

        public Guid UserId { get; set; }
        public DateTime CreateDate { get; set; }

        public string Text { get; set; }
        public bool IsApproved { get; set; }

        [ForeignKey(nameof(UserId))]
        public User Author { get; set; }
    }
}