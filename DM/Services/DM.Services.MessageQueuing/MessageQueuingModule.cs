using System;
using System.Reflection;
using Autofac;
using DM.Services.Core.Configuration;
using DM.Services.MessageQueuing.Consume;
using DM.Services.MessageQueuing.Processing;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Module = Autofac.Module;

namespace DM.Services.MessageQueuing
{
    /// <inheritdoc />
    public class MessageQueuingModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.IsClass)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.Register<IConnectionFactory>(x =>
                {
                    var mqConfiguration = x.Resolve<IOptions<ConnectionStrings>>().Value.MessageQueue;
                    return new ConnectionFactory
                    {
                        Endpoint = new AmqpTcpEndpoint(new Uri(mqConfiguration.Endpoint)),
                        UserName = mqConfiguration.UserName,
                        Password = mqConfiguration.Password,
                        VirtualHost = mqConfiguration.VirtualHost
                    };
                })
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterGeneric(typeof(EventProcessorAdapter<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(MessageProcessorWrapper<>))
                .AsImplementedInterfaces()
                .InstancePerDependency();
            builder.RegisterGeneric(typeof(MessageConsumer<>))
                .AsImplementedInterfaces()
                .SingleInstance();
            base.Load(builder);
        }
    }
}