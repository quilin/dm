namespace DM.Services.MessageQueuing
{
    /// <summary>
    /// Message process result that suggest the strategy to reply to MQ
    /// </summary>
    public enum ProcessResult
    {
        /// <summary>
        /// Successfully processed, MQ should get Ack
        /// </summary>
        Success = 0,

        /// <summary>
        /// Did not process, MQ should Nack and retry immediately
        /// </summary>
        RetryNeeded = 1,

        /// <summary>
        /// Did not process, MQ should Nack without retry
        /// </summary>
        Fail = 2
    }
}