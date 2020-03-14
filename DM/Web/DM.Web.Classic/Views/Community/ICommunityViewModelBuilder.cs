using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Web.Classic.Views.Community.CommunityUser;

namespace DM.Web.Classic.Views.Community
{
    public interface ICommunityViewModelBuilder
    {
        Task<CommunityViewModel> Build(int entityNumber, bool withInactive);
        Task<IEnumerable<CommunityUserViewModel>> BuildList(int entityNumber, bool withInactive);
    }
}