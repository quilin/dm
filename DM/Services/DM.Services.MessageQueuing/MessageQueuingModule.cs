using System;
using Autofac;
using DM.Services.Core;
using DM.Services.Core.Configuration;
using DM.Services.Core.Extensions;
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
            builder.RegisterDefaultTypes();

            builder.Register<IConnectionFactory>(x =>
                {
                    var mqConnectionString = x.Resolve<IOptions<ConnectionStrings>>().Value.MessageQueue;
                    return new ConnectionFactory
                    {
                        Endpoint = new AmqpTcpEndpoint(new Uri(mqConnectionString)),
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

            builder.RegisterModuleOnce<CoreModule>();

            base.Load(builder);
        }
    }
}