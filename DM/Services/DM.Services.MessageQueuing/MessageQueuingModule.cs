using System.Reflection;
using Autofac;
using DM.Services.MessageQueuing.Consume;
using DM.Services.MessageQueuing.Processing;
using RabbitMQ.Client;
using Module = Autofac.Module;

namespace DM.Services.MessageQueuing
{
    public class MessageQueuingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.IsClass)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.Register<IConnectionFactory>(x => new ConnectionFactory())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(MessageProcessorWrapper<>))
                .AsImplementedInterfaces()
                .InstancePerDependency();
            builder.RegisterGeneric(typeof(MessageConsumer<>))
                .AsImplementedInterfaces()
                .InstancePerDependency();
            base.Load(builder);
        }
    }
}