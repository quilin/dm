using DM.Services.Community.BusinessProcesses.Chat.Reading;

namespace DM.Web.Classic.Views.Chat.Message
{
    public interface IChatMessageViewModelBuilder
    {
        ChatMessageViewModel Build(ChatMessage message, ChatMessage[] messages, int i);
    }
}