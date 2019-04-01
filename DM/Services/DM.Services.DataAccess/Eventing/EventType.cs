namespace DM.Services.DataAccess.Eventing
{
    /// <summary>
    /// Message queue event type
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// Unknown event
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// New topic has been created
        /// </summary>
        NewTopic = 1
    }
}