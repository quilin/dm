using System;
using System.Collections.Generic;
using Autofac;
using DM.Services.Core.Configuration;
using DM.Services.Core.Extensions;
using DM.Services.MessageQueuing.Experiment.RabbitMq;
using DM.Services.MessageQueuing.Experiment.RabbitMq.Connection;
using DM.Services.MessageQueuing.Experiment.RabbitMq.Consuming;
using DM.Services.MessageQueuing.Experiment.RabbitMq.Producing;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DM.Services.MessageQueuing.Experiment
{
    /// <inheritdoc />
    public class MessageQueuingModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterDefaultTypes();

            builder.RegisterType<ConnectionPool>()
                .As<IProducerConnectionPool>()
                .SingleInstance();
            builder.RegisterType<ConnectionPool>()
                .As<IConsumerConnectionPool>()
                .SingleInstance();

            builder
                .Register(ctx =>
                {
                    var connectionStrings = ctx.Resolve<IOptions<ConnectionStrings>>().Value;
                    var assemblyDescription = ThisAssembly.GetName();
                    return new ConnectionFactory
                    {
                        Endpoint = new AmqpTcpEndpoint(connectionStrings.MessageQueue),
                        AutomaticRecoveryEnabled = true,
                        DispatchConsumersAsync = true,
                        ClientProperties = new Dictionary<string, object>
                        {
                            ["platform"] = ".NET",
                            ["platform-version"] = Environment.Version.ToString(),
                            ["product"] = assemblyDescription.Name,
                            ["version"] = assemblyDescription.Version?.ToString()
                        }
                    };
                })
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<Topology>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterGeneric(typeof(ConsumerStrategy<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder
                .RegisterGeneric(typeof(MessageProcessor<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder
                .RegisterGeneric(typeof(RabbitConsumer<,>))
                .AsSelf();

            builder
                .RegisterGeneric(typeof(RabbitProducer<>))
                .AsSelf();
            
            base.Load(builder);
        }
    }
}