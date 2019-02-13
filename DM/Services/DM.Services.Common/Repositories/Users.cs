using System;
using System.Linq;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Common.Repositories
{
    public static class Users
    {
        public static readonly Func<User, GeneralUser> GeneralProjection = u => new GeneralUser
        {
            UserId = u.UserId,
            Login = u.Login,
            Role = u.Role,
            AccessPolicy = u.AccessPolicy,
            LastVisitDate = u.LastVisitDate,
            ProfilePictureUrl = u.ProfilePictures.FirstOrDefault(p => !p.IsRemoved).VirtualPath,
            RatingDisabled = u.RatingDisabled,
            QualityRating = u.QualityRating,
            QuantityRating = u.QuantityRating
        };
    }
}