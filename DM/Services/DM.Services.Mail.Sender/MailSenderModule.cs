using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace DM.Services.Mail.Sender
{
    /// <inheritdoc />
    public class MailSenderModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.IsClass)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}