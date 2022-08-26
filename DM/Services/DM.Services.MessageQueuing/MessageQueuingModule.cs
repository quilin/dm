using Autofac;
using DM.Services.Core.Extensions;

namespace DM.Services.MessageQueuing
{
    /// <inheritdoc />
    public class MessageQueuingModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterDefaultTypes();
            base.Load(builder);
        }
    }
}