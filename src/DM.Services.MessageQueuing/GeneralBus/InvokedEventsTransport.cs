namespace DM.Services.MessageQueuing.GeneralBus;

/// <summary>
/// Information about invoked events transport
/// </summary>
public static class InvokedEventsTransport
{
    /// <summary>
    /// MQ exchange name to subscribe to
    /// </summary>
    public const string ExchangeName = "dm.events";
}