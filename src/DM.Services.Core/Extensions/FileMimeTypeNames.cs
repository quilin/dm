namespace DM.Services.Core.Extensions;

/// <summary>
/// Extension of file mime types
/// </summary>
public static class FileMimeTypeNames
{
    /// <summary>
    /// Image types
    /// </summary>
    public static class Image
    {
        /// <summary>
        /// Gif image mime type
        /// </summary>
        public const string Gif = "image/gif";

        /// <summary>
        /// Jpeg image mime type
        /// </summary>
        public const string Jpeg = "image/jpeg";

        /// <summary>
        /// Pjpeg image mime type
        /// </summary>
        public const string Pjpeg = "image/pjpeg";

        /// <summary>
        /// Png image mime type
        /// </summary>
        public const string Png = "image/png";

        /// <summary>
        /// Svg image mime type
        /// </summary>
        public const string Svg = "image/svg+xml";

        /// <summary>
        /// Tiff image mime type
        /// </summary>
        public const string Tiff = "image/tiff";
    }

    /// <summary>
    /// Application types
    /// </summary>
    public static class Application
    {
        /// <summary>
        /// Pdf application mime type
        /// </summary>
        public const string Pdf = "application/pdf";

        /// <summary>
        /// Zip application mime type
        /// </summary>
        public const string Zip = "application/zip";

        /// <summary>
        /// Gzip application mime type
        /// </summary>
        public const string Gzip = "application/gzip";
    }

    /// <summary>
    /// Text types
    /// </summary>
    public static class Text
    {
        /// <summary>
        /// Html text mime type
        /// </summary>
        public const string Html = "text/html";

        /// <summary>
        /// Plain text mime type
        /// </summary>
        public const string Plain = "text/plain";
    }
}