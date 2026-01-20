using AutoMapper;
using DM.Services.Community.BusinessProcesses.Chat.Reading;
using DbMessage = DM.Services.DataAccess.BusinessObjects.Common.ChatMessage;

namespace DM.Services.Community.Storage.Profiles;

/// <inheritdoc />
internal class ChatMessageProfile : Profile
{
    /// <inheritdoc />
    public ChatMessageProfile()
    {
        CreateMap<DbMessage, ChatMessage>();
    }
}