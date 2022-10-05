namespace DM.Services.Uploading.Configuration;

/// <summary>
/// Type of S3 service provider
/// </summary>
public enum S3Provider
{
    /// <summary>
    /// Basic AWS S3
    /// </summary>
    Amazon = 0,

    /// <summary>
    /// Heroku CloudCube
    /// </summary>
    CloudCube = 1,

    /// <summary>
    /// MinIO
    /// </summary>
    Minio = 2
}