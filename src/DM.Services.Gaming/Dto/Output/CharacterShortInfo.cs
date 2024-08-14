using System;

namespace DM.Services.Gaming.Dto.Output;

/// <inheritdoc />
public class CharacterShortInfo : CharacterShort
{
    /// <summary>
    /// Last character post in game
    /// </summary>
    public LastPost LastPost { get; set; }

    /// <summary>
    /// Posts count
    /// </summary>
    public int PostsCount { get; set; }
}

/// <summary>
/// Short model of last character post
/// </summary>
public class LastPost
{
    /// <summary>
    /// Post identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Room identifier
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Creating moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }
}