using System.Threading.Tasks;

namespace DM.Web.Classic.Views.Topic
{
    public interface ITopicActionsViewModelBuilder
    {
        Task<TopicActionsViewModel> Build(Services.Forum.Dto.Output.Topic topic);
    }
}