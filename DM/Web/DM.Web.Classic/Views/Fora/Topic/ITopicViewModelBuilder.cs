namespace DM.Web.Classic.Views.Fora.Topic
{
    public interface ITopicViewModelBuilder
    {
        TopicViewModel Build(Services.Forum.Dto.Output.Topic topic);
    }
}