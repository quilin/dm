using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using DM.Services.Common.Configuration;
using Microsoft.Extensions.Options;

namespace DM.Services.Common.BusinessProcesses.Uploads
{
    /// <inheritdoc />
    public class Uploader : IUploader
    {
        private readonly Lazy<IAmazonS3> client;
        private readonly CdnConfiguration cdnConfiguration;

        /// <inheritdoc />
        public Uploader(
            IOptions<CdnConfiguration> cdnOptions,
            Lazy<IAmazonS3> client)
        {
            this.client = client;
            cdnConfiguration = cdnOptions.Value;
        }

        /// <inheritdoc />
        public async Task<string> Upload(Func<Stream> fileStream, string fileName)
        {
            using var stream = fileStream();
            var objectKey = $"{cdnConfiguration.Folder}/{fileName}";
            await client.Value.UploadObjectFromStreamAsync(cdnConfiguration.BucketName, objectKey, stream, null);
            return new UriBuilder(new Uri(cdnConfiguration.Url)) {Path = objectKey}.ToString();
        }
    }
}