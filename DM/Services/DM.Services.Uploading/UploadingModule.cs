using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Autofac;
using DM.Services.Authentication;
using DM.Services.Core;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.Uploading.Configuration;
using Microsoft.Extensions.Options;
using Module = Autofac.Module;

namespace DM.Services.Uploading
{
    /// <inheritdoc />
    public class UploadingModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterDefaultTypes();
            builder.RegisterMapper();
            builder.Register<IAmazonS3>(ctx =>
                {
                    var cdnConfiguration = ctx.Resolve<IOptions<CdnConfiguration>>().Value;
                    return new AmazonS3Client(
                        new BasicAWSCredentials(cdnConfiguration.AccessKey, cdnConfiguration.SecretKey),
                        new AmazonS3Config
                        {
                            ServiceURL = cdnConfiguration.Url,
                            RegionEndpoint = RegionEndpoint.GetBySystemName(cdnConfiguration.Region)
                        });
                })
                .AsSelf()
                .SingleInstance();

            builder.RegisterModuleOnce<CoreModule>();
            builder.RegisterModuleOnce<AuthenticationModule>();
            builder.RegisterModuleOnce<DataAccessModule>();

            base.Load(builder);
        }
    }
}