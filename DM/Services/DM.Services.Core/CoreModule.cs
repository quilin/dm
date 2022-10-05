using Autofac;
using DM.Services.Core.Extensions;
using DM.Services.Core.Implementation.CorrelationToken;

namespace DM.Services.Core;

/// <inheritdoc />
public class CoreModule : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterDefaultTypes();

        builder.RegisterType<CorrelationTokenProvider>()
            .AsSelf()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        base.Load(builder);
    }
}