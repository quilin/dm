using Autofac;
using DM.Services.Authentication;
using DM.Services.Core;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;

namespace DM.Services.Notifications;

/// <inheritdoc />
public class NotificationsModule : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterDefaultTypes();
        builder.RegisterMapper();

        builder.RegisterModuleOnce<CoreModule>();
        builder.RegisterModuleOnce<DataAccessModule>();
        builder.RegisterModuleOnce<AuthenticationModule>();

        base.Load(builder);
    }
}