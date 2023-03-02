using Microsoft.Extensions.Configuration;

namespace DM.Services.MessageQueuing;

/// <summary>
/// Configuration for message queue connection
/// </summary>
public class RabbitMqConfiguration
{
    /// <summary>
    /// Broker endpoint url
    /// </summary>
    public string Endpoint { get; set; }

    /// <summary>
    /// Broker virtual host
    /// </summary>
    public string VirtualHost { get; set; }

    /// <summary>
    /// User name
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Bind configuration parameters to new instance
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static RabbitMqConfiguration From(IConfiguration configuration)
    {
        var result = new RabbitMqConfiguration();
        configuration.GetSection(nameof(RabbitMqConfiguration)).Bind(result);
        return result;
    }
}