using Autofac;
using DM.Services.Authentication;
using DM.Services.Common;
using DM.Services.Core;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.MessageQueuing;

namespace DM.Services.Gaming;

/// <inheritdoc />
public class GamingModule : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterDefaultTypes();
        builder.RegisterMapper();

        builder.RegisterModuleOnce<CoreModule>();
        builder.RegisterModuleOnce<DataAccessModule>();
        builder.RegisterModuleOnce<AuthenticationModule>();
        builder.RegisterModuleOnce<CommonModule>();
        builder.RegisterModuleOnce<MessageQueuingModule>();

        base.Load(builder);
    }
}