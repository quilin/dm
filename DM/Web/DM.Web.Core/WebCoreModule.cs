using Autofac;
using DM.Services.Authentication;
using DM.Services.Core;
using DM.Services.Core.Extensions;

namespace DM.Web.Core
{
    /// <inheritdoc />
    public class WebCoreModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterDefaultTypes();

            builder.RegisterModuleOnce<CoreModule>();
            builder.RegisterModuleOnce<AuthenticationModule>();

            base.Load(builder);
        }
    }
}