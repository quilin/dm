using AutoMapper;
using DbMessage = DM.Services.DataAccess.BusinessObjects.Common.ChatMessage;

namespace DM.Services.Community.BusinessProcesses.Chat.Reading;

/// <inheritdoc />
internal class ChatMessageProfile : Profile
{
    /// <inheritdoc />
    public ChatMessageProfile()
    {
        CreateMap<DbMessage, ChatMessage>();
    }
}