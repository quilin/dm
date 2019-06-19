using DM.Services.Core.Dto;

namespace DM.Web.Classic.Views.Shared.User
{
    public interface IUserRatingFactory
    {
        UserRating Create(IUser user);
    }
}