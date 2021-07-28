using AutoMapper;
using DM.Services.DataAccess.BusinessObjects.Notifications;

namespace DM.Services.Notifications.Dto
{
    /// <inheritdoc />
    public class NotificationProfile : Profile
    {
        /// <inheritdoc />
        public NotificationProfile()
        {
            CreateMap<Notification, RealtimeNotification>()
                .ForMember(d => d.RecipientIds, s => s.MapFrom(n => n.UsersInterested));
        }
    }
}