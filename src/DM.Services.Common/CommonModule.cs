using Autofac;
using DM.Services.Authentication;
using DM.Services.Core;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using Module = Autofac.Module;

namespace DM.Services.Common;

/// <inheritdoc />
public class CommonModule : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterDefaultTypes();
        builder.RegisterMapper();

        builder.RegisterModuleOnce<CoreModule>();
        builder.RegisterModuleOnce<AuthenticationModule>();
        builder.RegisterModuleOnce<DataAccessModule>();

        base.Load(builder);
    }
}