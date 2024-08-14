namespace DM.Services.Uploading.Configuration;

/// <summary>
/// CDN configuration
/// </summary>
public class CdnConfiguration
{
    /// <summary>
    /// Service URL
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Public CDN URL
    /// </summary>
    public string PublicUrl { get; set; }

    /// <summary>
    /// AWS region
    /// </summary>
    public string Region { get; set; }

    /// <summary>
    /// Files folder
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// Private access key
    /// </summary>
    public string SecretKey { get; set; }

    /// <summary>
    /// Public access key
    /// </summary>
    public string AccessKey { get; set; }

    /// <summary>
    /// Relative file destination
    /// </summary>
    public string Folder { get; set; }

    /// <summary>
    /// CDN is MinIO
    /// </summary>
    public S3Provider Provider { get; set; }
}