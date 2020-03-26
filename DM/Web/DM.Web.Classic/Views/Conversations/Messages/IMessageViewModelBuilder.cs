using DM.Services.Community.BusinessProcesses.Messaging.Reading;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Conversations.Messages
{
    public interface IMessageViewModelBuilder
    {
        MessageViewModel Build(Message message, Message[] messages, int index);
        MessageViewModel Build(Message message, UserViewModel collocutor);
    }
}