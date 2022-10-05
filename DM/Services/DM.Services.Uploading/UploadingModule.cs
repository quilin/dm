using System.Collections.Generic;
using System.Linq;
using Autofac;
using DM.Services.Authentication;
using DM.Services.Core;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.Uploading.Configuration;
using Module = Autofac.Module;

namespace DM.Services.Uploading;

/// <inheritdoc />
public class UploadingModule : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterDefaultTypes();
        builder.RegisterMapper();
        builder.Register(ctx =>
            {
                var amazonS3ClientProviders = ctx
                    .Resolve<IEnumerable<IAmazonS3ClientProvider>>();
                return amazonS3ClientProviders
                    .First(p => p.CanBeUsed())
                    .GetClient();
            })
            .AsSelf()
            .SingleInstance();

        builder.RegisterModuleOnce<CoreModule>();
        builder.RegisterModuleOnce<AuthenticationModule>();
        builder.RegisterModuleOnce<DataAccessModule>();

        base.Load(builder);
    }
}