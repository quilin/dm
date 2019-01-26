using System;

namespace DM.Services.DataAccess.BusinessObjects.Users
{
    public interface IPublicUser
    {
        Guid UserId { get; }
        string Login { get; }
        UserRole Role { get; }

        DateTime? LastVisitDate { get; }

        bool RatingDisabled { get; }
        int QualityRating { get; }
        int QuantityRating { get; }
    }
}