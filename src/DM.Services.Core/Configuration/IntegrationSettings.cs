namespace DM.Services.Core.Configuration;

/// <summary>
/// Integration between DM sites configuration
/// </summary>
public class IntegrationSettings
{
    /// <summary>
    /// DM.API URL
    /// </summary>
    public string ApiUrl { get; set; }

    /// <summary>
    /// DM website URL
    /// </summary>
    public string WebUrl { get; set; }

    /// <summary>
    /// DM administration tools website URL
    /// </summary>
    public string AdminUrl { get; set; }

    /// <summary>
    /// DM mobile site URL
    /// </summary>
    public string MobileUrl { get; set; }

    /// <summary>
    /// External URLs for CORS policies
    /// </summary>
    public string[] CorsUrls { get; set; }
}