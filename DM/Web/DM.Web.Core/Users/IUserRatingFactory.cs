using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Web.Core.Users
{
    public interface IUserRatingFactory
    {
        UserRating Create(IUser user);
    }
}