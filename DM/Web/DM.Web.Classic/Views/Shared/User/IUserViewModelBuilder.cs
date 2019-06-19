using DM.Services.Core.Dto;

namespace DM.Web.Classic.Views.Shared.User
{
    public interface IUserViewModelBuilder
    {
        UserViewModel Build(GeneralUser user);
    }
}