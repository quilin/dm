using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Options;

namespace DM.Services.Uploading.Configuration;

internal class MinioS3ClientProvider(IOptions<CdnConfiguration> cdnConfiguration) : IAmazonS3ClientProvider
{
    private readonly CdnConfiguration cdnConfiguration = cdnConfiguration.Value;

    public IAmazonS3 GetClient()
    {
        return new AmazonS3Client(
            new BasicAWSCredentials(cdnConfiguration.AccessKey, cdnConfiguration.SecretKey),
            new AmazonS3Config
            {
                ServiceURL = cdnConfiguration.Url,
                AuthenticationRegion = cdnConfiguration.Region,
                ForcePathStyle = true
            });
    }

    public bool CanBeUsed() => cdnConfiguration.Provider == S3Provider.Minio;
}