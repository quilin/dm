namespace DM.Services.MessageQueuing.RabbitMq.Connection
{
    internal enum ConnectionBusinessStatus
    {
        Unknown = 0,
        Idle = 1,
        Free = 2,
        Working = 3,
        Busy = 4,
        Full = 5
    }
}