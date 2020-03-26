using DM.Services.Community.BusinessProcesses.Messaging.Reading;

namespace DM.Web.Classic.Views.Conversations.List.Conversations
{
    public interface IConversationViewModelBuilder
    {
        ConversationViewModel Build(Conversation conversation);
    }
}