using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.EditTopic
{
    public interface IEditTopicFormBuilder
    {
        Task<EditTopicForm> Build(Guid topicId);
    }
}