using System;

namespace DM.Web.Classic.Views.EditTopic
{
    public interface IEditTopicFormBuilder
    {
        EditTopicForm Build(Guid topicId);
    }
}