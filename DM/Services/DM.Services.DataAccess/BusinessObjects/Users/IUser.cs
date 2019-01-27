using System;

namespace DM.Services.DataAccess.BusinessObjects.Users
{
    public interface IUser
    {
        Guid UserId { get; }
        string Login { get; }
        UserRole Role { get; }
        AccessPolicy AccessPolicy { get; }

        DateTime? LastVisitDate { get; }

        bool RatingDisabled { get; }
        int QualityRating { get; }
        int QuantityRating { get; }
    }
}