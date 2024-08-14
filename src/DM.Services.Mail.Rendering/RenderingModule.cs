using Autofac;
using DM.Services.Core.Extensions;

namespace DM.Services.Mail.Rendering;

/// <inheritdoc />
public class RenderingModule : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterDefaultTypes();

        base.Load(builder);
    }
}