using System;
using System.Collections.Generic;
using Autofac;
using DM.Services.Core.Extensions;
using DM.Services.MessageQueuing.Building;
using DM.Services.MessageQueuing.RabbitMq;
using DM.Services.MessageQueuing.RabbitMq.Configuration;
using DM.Services.MessageQueuing.RabbitMq.Connection;
using DM.Services.MessageQueuing.RabbitMq.Consuming;
using DM.Services.MessageQueuing.RabbitMq.Producing;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DM.Services.MessageQueuing
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
                    var connectionParameters = ctx.Resolve<IOptions<RabbitMqConfiguration>>().Value;
                    var assemblyDescription = ThisAssembly.GetName();

                    var endpointUri = new Uri(connectionParameters.Endpoint);
                    return new ConnectionFactory
                    {
                        Endpoint = new AmqpTcpEndpoint(endpointUri),
                        UserName = connectionParameters.UserName,
                        Password = connectionParameters.Password,
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
                .RegisterGeneric(typeof(MessageProcessor<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder
                .RegisterGeneric(typeof(RabbitConsumer<,>))
                .AsSelf();

            builder
                .RegisterGeneric(typeof(RabbitProducer<>))
                .AsSelf();

            builder
                .RegisterGeneric(typeof(ConsumerBuilder<>))
                .AsImplementedInterfaces()
                .SingleInstance();
            builder
                .RegisterGeneric(typeof(ProducerBuilder<,>))
                .AsImplementedInterfaces()
                .SingleInstance();

            base.Load(builder);
        }
    }
}