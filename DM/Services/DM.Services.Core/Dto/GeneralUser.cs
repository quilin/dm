using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Core.Dto
{
    /// <summary>
    /// DTO model for a user in almost any circumstances
    /// </summary>
    public class GeneralUser : IUser
    {
        /// <inheritdoc />
        public Guid UserId { get; set; }

        /// <inheritdoc />
        public string Login { get; set; }

        /// <inheritdoc />
        public UserRole Role { get; set; }

        /// <inheritdoc />
        public AccessPolicy AccessPolicy { get; set; }

        /// <inheritdoc />
        public DateTime? LastVisitDate { get; set; }

        /// <summary>
        /// URL of current profile picture
        /// </summary>
        public string ProfilePictureUrl { get; set; }

        /// <inheritdoc />
        public bool RatingDisabled { get; set; }

        /// <inheritdoc />
        public int QualityRating { get; set; }

        /// <inheritdoc />
        public int QuantityRating { get; set; }

        /// <summary>
        /// Whether user is authenticated or not
        /// </summary>
        public bool IsAuthenticated => Role != UserRole.Guest;
    }
}