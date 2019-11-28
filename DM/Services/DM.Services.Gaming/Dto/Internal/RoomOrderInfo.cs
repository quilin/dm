using System;

namespace DM.Services.Gaming.Dto.Internal
{
    /// <summary>
    /// DTO model for rooms linked list
    /// </summary>
    public class RoomOrderInfo
    {
        /// <summary>
        /// Room identifier
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Room order number
        /// </summary>
        public double OrderNumber { get; set; }
    }
}