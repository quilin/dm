using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using DM.Services.Uploading.Configuration;
using Microsoft.Extensions.Options;

namespace DM.Services.Uploading.BusinessProcesses.Cdn;

/// <inheritdoc />
internal class Uploader : IUploader
{
    private readonly CdnConfiguration cdnConfiguration;
    private readonly Lazy<IAmazonS3> client;

    /// <inheritdoc />
    public Uploader(
        IOptions<CdnConfiguration> cdnOptions,
        Lazy<IAmazonS3> client)
    {
        cdnConfiguration = cdnOptions.Value;
        this.client = client;
    }

    /// <inheritdoc />
    public async Task<string> Upload(Func<Stream> streamAccessor, string fileName)
    {
        var stream = streamAccessor();
        var objectKey = string.IsNullOrEmpty(cdnConfiguration.Folder)
            ? fileName
            : $"{cdnConfiguration.Folder}/{fileName}";
        await client.Value.UploadObjectFromStreamAsync(cdnConfiguration.BucketName, objectKey, stream, null);
        return new UriBuilder(new Uri(cdnConfiguration.PublicUrl))
        {
            Path = $"{cdnConfiguration.BucketName}/{objectKey}"
        }.ToString();
    }
}