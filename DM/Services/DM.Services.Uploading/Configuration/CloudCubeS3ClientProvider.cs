using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Options;

namespace DM.Services.Uploading.Configuration;

internal class CloudCubeS3ClientProvider : IAmazonS3ClientProvider
{
    private readonly CdnConfiguration cdnConfiguration;

    public CloudCubeS3ClientProvider(
        IOptions<CdnConfiguration> cdnConfiguration)
    {
        this.cdnConfiguration = cdnConfiguration.Value;
    }

    public IAmazonS3 GetClient() => new AmazonS3Client(
        new BasicAWSCredentials(cdnConfiguration.AccessKey, cdnConfiguration.SecretKey),
        new AmazonS3Config
        {
            ServiceURL = cdnConfiguration.Url,
            RegionEndpoint = RegionEndpoint.GetBySystemName(cdnConfiguration.Region)
        });

    public bool CanBeUsed() => cdnConfiguration.Provider == S3Provider.CloudCube;
}