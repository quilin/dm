using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.Topic
{
    public interface ITopicViewModelBuilder
    {
        Task<TopicViewModel> Build(Guid topicId, int entityNumber);
    }
}