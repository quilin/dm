using DM.Services.DataAccess.BusinessObjects.Users;

namespace Web.Core.Users
{
    public class UserRatingFactory : IUserRatingFactory
    {
        public UserRating Create(IUser user)
        {
            return new UserRating
            {
                Quality = user.QualityRating,
                Quantity = user.QuantityRating,
                Disabled = user.RatingDisabled
            };
        }
    }
}