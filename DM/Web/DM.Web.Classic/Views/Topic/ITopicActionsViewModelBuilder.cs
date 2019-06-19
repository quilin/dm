namespace DM.Web.Classic.Views.Topic
{
    public interface ITopicActionsViewModelBuilder
    {
        TopicActionsViewModel Build(Services.Forum.Dto.Output.Topic topic);
    }
}