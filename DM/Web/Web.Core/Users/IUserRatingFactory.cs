using DM.Services.DataAccess.BusinessObjects.Users;

namespace Web.Core.Users
{
    public interface IUserRatingFactory
    {
        UserRating Create(IUser user);
    }
}