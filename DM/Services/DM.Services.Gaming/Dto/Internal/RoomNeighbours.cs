namespace DM.Services.Gaming.Dto.Internal
{
    /// <summary>
    /// DTO model for neighbour rooms
    /// </summary>
    public class RoomNeighbours
    {
        /// <summary>
        /// Previous room info
        /// </summary>
        public RoomOrderInfo PreviousRoom { get; set; }

        /// <summary>
        /// Next room info
        /// </summary>
        public RoomOrderInfo NextRoom { get; set; }
    }
}