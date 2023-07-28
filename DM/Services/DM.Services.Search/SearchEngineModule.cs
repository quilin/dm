using System;
using Autofac;
using DM.Services.Authentication;
using DM.Services.Core;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.SearchEngine;
using DM.Services.Search.Configuration;
using Microsoft.Extensions.Options;
using OpenSearch.Client;
using Module = Autofac.Module;

namespace DM.Services.Search;

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
                var configuration = x.Resolve<IOptions<SearchEngineConfiguration>>().Value;
                return new ConnectionSettings(new Uri(configuration.Endpoint))
                    .BasicAuthentication(configuration.Username, configuration.Password)
                    .DefaultMappingFor<SearchEntity>(m => m
                        .IndexName(SearchEngineConfiguration.IndexName));
            })
            .SingleInstance();

        builder.Register(x => new OpenSearchClient(x.Resolve<ConnectionSettings>()))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterModuleOnce<CoreModule>();
        builder.RegisterModuleOnce<DataAccessModule>();
        builder.RegisterModuleOnce<AuthenticationModule>();

        base.Load(builder);
    }
}