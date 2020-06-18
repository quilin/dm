using System;
using Autofac;
using DM.Services.Authentication;
using DM.Services.Core;
using DM.Services.Core.Configuration;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.SearchEngine;
using DM.Services.Search.Configuration;
using Microsoft.Extensions.Options;
using Nest;
using Module = Autofac.Module;

namespace DM.Services.Search
{
    /// <inheritdoc />
    public class SearchEngineModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterDefaultTypes();
            builder.RegisterMapper();

            builder.Register(x =>
                {
                    var connectionStrings = x.Resolve<IOptions<ConnectionStrings>>().Value;
                    return new ConnectionSettings(new Uri(connectionStrings.SearchEngine))
                        .DefaultMappingFor<SearchEntity>(m => m
                            .IndexName(SearchEngineConfiguration.IndexName));
                })
                .SingleInstance();

            builder.Register(x => new ElasticClient(x.Resolve<ConnectionSettings>()))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterModuleOnce<CoreModule>();
            builder.RegisterModuleOnce<DataAccessModule>();
            builder.RegisterModuleOnce<AuthenticationModule>();

            base.Load(builder);
        }
    }
}