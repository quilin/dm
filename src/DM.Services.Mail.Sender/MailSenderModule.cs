using Autofac;
using DM.Services.Core.Extensions;
using DM.Services.MessageQueuing;
using Module = Autofac.Module;

namespace DM.Services.Mail.Sender;

/// <inheritdoc />
public class MailSenderModule : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterDefaultTypes();

        builder.RegisterModuleOnce<MessageQueuingModule>();

        base.Load(builder);
    }
}