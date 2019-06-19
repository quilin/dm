using DM.Services.Core.Dto;

namespace DM.Web.Classic.Views.Community.CommunityUser
{
    public interface ICommunityUserViewModelBuilder
    {
        CommunityUserViewModel Build(GeneralUser user, int index);
    }
}