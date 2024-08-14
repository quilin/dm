using Autofac;
using DM.Services.Authentication;
using DM.Services.Common;
using DM.Services.Core;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.Mail.Rendering;
using DM.Services.Mail.Sender;
using DM.Services.Uploading;

namespace DM.Services.Community;

/// <inheritdoc />
public class CommunityModule : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterDefaultTypes();
        builder.RegisterMapper();

        builder.RegisterModuleOnce<CoreModule>();
        builder.RegisterModuleOnce<AuthenticationModule>();
        builder.RegisterModuleOnce<CommonModule>();
        builder.RegisterModuleOnce<UploadingModule>();
        builder.RegisterModuleOnce<DataAccessModule>();
        builder.RegisterModuleOnce<MailSenderModule>();
        builder.RegisterModuleOnce<RenderingModule>();

        base.Load(builder);
    }
}