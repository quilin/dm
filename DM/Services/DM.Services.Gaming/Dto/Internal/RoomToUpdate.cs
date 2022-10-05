using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Dto.Internal;

/// <summary>
/// Internal DTO model for room updating
/// </summary>
internal class RoomToUpdate : Room
{
    /// <summary>
    /// Room parent game
    /// </summary>
    public Game Game { get; set; }

    /// <summary>
    /// Previous room order info
    /// </summary>
    public RoomOrderInfo PreviousRoom { get; set; }

    /// <summary>
    /// Next room order info
    /// </summary>
    public RoomOrderInfo NextRoom { get; set; }
}