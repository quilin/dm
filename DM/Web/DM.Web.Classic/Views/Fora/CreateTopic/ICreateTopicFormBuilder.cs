using DM.Services.Forum.Dto.Output;

namespace DM.Web.Classic.Views.Fora.CreateTopic
{
    public interface ICreateTopicFormBuilder
    {
        CreateTopicForm Build(Forum forum);
    }
}