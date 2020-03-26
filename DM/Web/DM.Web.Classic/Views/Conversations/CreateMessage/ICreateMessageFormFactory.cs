using DM.Services.Community.BusinessProcesses.Messaging.Reading;

namespace DM.Web.Classic.Views.Conversations.CreateMessage
{
    public interface ICreateMessageFormFactory
    {
        CreateMessageForm Create(Conversation conversation);
    }
}