using DM.Web.Classic.Views.Fora.Topic;
using DM.Services.Forum.Dto.Output;

namespace DM.Web.Classic.Views.Fora
{
    public interface IForumViewModelBuilder
    {
        ForumViewModel Build(Forum forum, int entityNumber);
        TopicViewModel[] BuildList(Forum forum, int entityNumber);
    }
}