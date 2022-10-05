using System;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.DataAccess.BusinessObjects.Games.Posts;

/// <summary>
/// DAL model for post in chat room
/// </summary>
public class ChatPost : ChatMessage
{
    /// <summary>
    /// Room identifier
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Character identifier
    /// </summary>
    public Guid? CharacterId { get; set; }
}