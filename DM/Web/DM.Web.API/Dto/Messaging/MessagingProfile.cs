using AutoMapper;
using DM.Services.Community.BusinessProcesses.Chat.Creating;
using DM.Services.Community.BusinessProcesses.Messaging.Creating;
using DtoConversation = DM.Services.Community.BusinessProcesses.Messaging.Reading.Conversation;
using DtoMessage = DM.Services.Community.BusinessProcesses.Messaging.Reading.Message;
using DtoChatMessage = DM.Services.Community.BusinessProcesses.Chat.Reading.ChatMessage;

namespace DM.Web.API.Dto.Messaging;

/// <inheritdoc />
internal class MessagingProfile : Profile
{
    /// <inheritdoc />
    public MessagingProfile()
    {
        CreateMap<DtoConversation, Conversation>();
        CreateMap<DtoMessage, Message>();
        CreateMap<Message, CreateMessage>();

        CreateMap<DtoChatMessage, ChatMessage>();
        CreateMap<ChatMessage, CreateChatMessage>();
    }
}