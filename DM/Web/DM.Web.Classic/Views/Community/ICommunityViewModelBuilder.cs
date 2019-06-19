using DM.Web.Classic.Views.Community.CommunityUser;

namespace DM.Web.Classic.Views.Community
{
    public interface ICommunityViewModelBuilder
    {
        CommunityViewModel Build(int entityNumber, bool withInactive);
        CommunityUserViewModel[] BuildList(int entityNumber, bool withInactive);
    }
}