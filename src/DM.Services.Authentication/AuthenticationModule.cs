using Autofac;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.MessageQueuing;
using Module = Autofac.Module;

namespace DM.Services.Authentication;

/// <inheritdoc />
public class AuthenticationModule : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterDefaultTypes();
        builder.RegisterMapper();

        // Identity provider should be scoped per request
        builder.RegisterType<IdentityProvider>()
            .AsSelf()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterModuleOnce<CoreModule>();
        builder.RegisterModuleOnce<DataAccessModule>();
        builder.RegisterModuleOnce<MessageQueuingModule>();

        base.Load(builder);
    }
}