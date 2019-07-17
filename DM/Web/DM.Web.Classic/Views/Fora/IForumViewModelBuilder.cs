using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Web.Classic.Views.Fora.Topic;
using DM.Services.Forum.Dto.Output;

namespace DM.Web.Classic.Views.Fora
{
    public interface IForumViewModelBuilder
    {
        Task<ForumViewModel> Build(Forum forum, int entityNumber);
        Task<IEnumerable<TopicViewModel>> BuildList(Forum forum, int entityNumber);
    }
}