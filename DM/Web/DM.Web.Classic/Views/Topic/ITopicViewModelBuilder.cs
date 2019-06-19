using System;

namespace DM.Web.Classic.Views.Topic
{
    public interface ITopicViewModelBuilder
    {
        TopicViewModel Build(Guid topicId, int entityNumber);
    }
}