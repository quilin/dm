using System.Linq;
using AutoMapper;
using DbConversation = DM.Services.DataAccess.BusinessObjects.Messaging.Conversation;
using DbMessage = DM.Services.DataAccess.BusinessObjects.Messaging.Message;

namespace DM.Services.Community.BusinessProcesses.Messaging.Reading;

/// <inheritdoc />
internal class MessagingProfile : Profile
{
    /// <inheritdoc />
    public MessagingProfile()
    {
        CreateMap<DbConversation, Conversation>()
            .ForMember(d => d.Id, s => s.MapFrom(c => c.ConversationId))
            .ForMember(d => d.Participants, s => s.MapFrom(c => c.UserLinks
                .Where(l => !l.IsRemoved)
                .Select(l => l.User)));

        CreateMap<DbMessage, Message>()
            .ForMember(d => d.Id, s => s.MapFrom(m => m.MessageId));
    }
}