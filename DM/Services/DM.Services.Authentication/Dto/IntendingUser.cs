using System.Collections.Generic;
using System.Linq;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Dto
{
    public class IntendingUser : AuthenticatingUser
    {
        public IntendingUser(IUser user, IEnumerable<IntentionBan> activeBans)
        {
            UserId = user.UserId;
            Login = user.Login;
            LastVisitDate = user.LastVisitDate;
            Role = user.Role;
            AccessPolicy = user.AccessPolicy;
            RatingDisabled = user.RatingDisabled;
            QualityRating = user.QualityRating;
            QuantityRating = user.QuantityRating;

            GeneralAccessPolicy = activeBans.Aggregate(user.AccessPolicy, (seed, ban) => seed | ban.AccessPolicy);
        }

        public AccessPolicy GeneralAccessPolicy { get; }
    }
}