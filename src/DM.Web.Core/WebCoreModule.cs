using Autofac;
using DM.Services.Authentication;
using DM.Services.Core;
using DM.Services.Core.Extensions;
using DM.Web.Core.Hubs;

namespace DM.Web.Core;

/// <inheritdoc />
public class WebCoreModule : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterDefaultTypes();

        // Should be singleton for it is the API state storage that traces active WS user connections
        builder.RegisterType<UserConnectionService>()
            .AsImplementedInterfaces()
            .SingleInstance();

        builder.RegisterModuleOnce<CoreModule>();
        builder.RegisterModuleOnce<AuthenticationModule>();

        base.Load(builder);
    }
}