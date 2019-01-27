using System;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.DataAccess.BusinessObjects.Games.Posts
{
    public class ChatPost : ChatMessage
    {
        public Guid? CharacterId { get; set; }
        public Guid RoomId { get; set; }
    }
}