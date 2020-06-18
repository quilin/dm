﻿using System;
using System.Collections.Generic;
using Autofac;
using DM.Services.Core;
using DM.Services.Core.Configuration;
using DM.Services.Core.Extensions;
using DM.Services.Core.Logging;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Consume;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ConfigurationFactory = DM.Services.Core.Configuration.ConfigurationFactory;

namespace DM.Services.Mail.Sender.Consumer
{
    /// <summary>
    /// Search consumer API configuration
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = ConfigurationFactory.Default;
            services
                .AddOptions()
                .Configure<EmailConfiguration>(configuration.GetSection(nameof(EmailConfiguration)).Bind)
                .Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .AddDmLogging("DM.MailSender.Consumer");

            services.AddHealthChecks();

            services.AddMvc();
        }

        /// <summary>
        /// Configure application container
        /// </summary>
        /// <param name="builder">Container builder</param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterDefaultTypes();

            builder.RegisterModuleOnce<CoreModule>();
            builder.RegisterModuleOnce<MailSenderModule>();
            builder.RegisterModuleOnce<MessageQueuingModule>();
        }

        /// <summary>
        /// Ready to work
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="consumer"></param>
        public void Configure(IApplicationBuilder applicationBuilder,
            IMessageConsumer<MailLetter> consumer)
        {
            Console.WriteLine("[🚴] Starting search engine consumer");

            var configuration = new MessageConsumeConfiguration
            {
                ExchangeName = "dm.mail.sending",
                RoutingKeys = new[] {"#"},
                QueueName = "dm.mail.sending",
                Arguments = new Dictionary<string, object>
                {
                    ["x-dead-letter-exchange"] = "dm.mail.unsent"
                },
                PrefetchCount = 1
            };
            consumer.Consume(configuration);

            Console.WriteLine($"[👂] Consumer is listening to {configuration.QueueName} queue");

            applicationBuilder
                .UseRouting()
                .UseHealthChecks("/_health")
                .UseEndpoints(route => route.MapControllers());
        }
    }
}