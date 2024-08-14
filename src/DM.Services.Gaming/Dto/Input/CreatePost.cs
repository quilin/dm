using System;

namespace DM.Services.Gaming.Dto.Input;

/// <summary>
/// DTO model for post creating
/// </summary>
public class CreatePost
{
    /// <summary>
    /// Room identifier
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Character identifier
    /// </summary>
    public Guid? CharacterId { get; set; }

    /// <summary>
    /// Text
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Commentary
    /// </summary>
    public string Commentary { get; set; }

    /// <summary>
    /// Message to master
    /// </summary>
    public string MasterMessage { get; set; }
}