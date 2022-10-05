using System;
using DM.Services.Core.Dto;

namespace DM.Services.Gaming.Dto.Output;

/// <summary>
/// DTO model for game post
/// </summary>
public class Post
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Room identifier
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Post author
    /// </summary>
    public GeneralUser Author { get; set; }

    /// <summary>
    /// Post last update author
    /// </summary>
    public GeneralUser LastUpdateAuthor { get; set; }

    /// <summary>
    /// Short character information
    /// </summary>
    public CharacterShort Character { get; set; }

    /// <summary>
    /// Creating moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Last update moment
    /// </summary>
    public DateTimeOffset? LastUpdateDate { get; set; }

    /// <summary>
    /// Text
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Commentary
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// Message to master or master note
    /// </summary>
    public string MasterMessage { get; set; }
}