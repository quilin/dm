using DM.Services.Core.Dto;

namespace DM.Web.Classic.Views.Shared.User
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