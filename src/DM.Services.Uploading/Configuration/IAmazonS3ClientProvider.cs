using Amazon.S3;

namespace DM.Services.Uploading.Configuration;

internal interface IAmazonS3ClientProvider
{
    IAmazonS3 GetClient();
    bool CanBeUsed();
}