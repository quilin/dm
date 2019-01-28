using System;
using System.Linq.Expressions;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Dto
{
    public class AuthenticatingUser : IUser
    {
        public Guid UserId { get; set; }
        public string Login { get; set; }
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

        public static Expression<Func<User, AuthenticatingUser>> FromDal => user => new AuthenticatingUser
        {
            UserId = user.UserId,
            Login = user.Login,
            Role = user.Role,
            AccessPolicy = user.AccessPolicy,
            LastVisitDate = user.LastVisitDate,
            RatingDisabled = user.RatingDisabled,
            QualityRating = user.QualityRating,
            QuantityRating = user.QuantityRating,
            Activated = user.Activated,
            Salt = user.Salt,
            PasswordHash = user.PasswordHash,
            IsRemoved = user.IsRemoved
        };
    }
}