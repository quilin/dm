using AutoMapper;
using DM.Services.Community.BusinessProcesses.Messaging.Creating;
using DtoConversation = DM.Services.Community.BusinessProcesses.Messaging.Reading.Conversation;
using DtoMessage = DM.Services.Community.BusinessProcesses.Messaging.Reading.Message;

namespace DM.Web.API.Dto.Messaging
{
    /// <inheritdoc />
    public class MessagingProfile : Profile
    {
        /// <inheritdoc />
        public MessagingProfile()
        {
            CreateMap<DtoConversation, Conversation>();
            CreateMap<DtoMessage, Message>();
            CreateMap<Message, CreateMessage>();
        }
    }
}