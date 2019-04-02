using System;
using System.Reflection;
using Autofac;
using DM.Services.Core.Configuration;
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
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.IsClass)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.Register(x =>
                {
                    var connectionStrings = x.Resolve<IOptions<ConnectionStrings>>().Value;
                    var searchEngineConfiguration = x.Resolve<IOptions<SearchEngineConfiguration>>().Value;
                    return new ConnectionSettings(new Uri(connectionStrings.DmSearchEngine))
                        .DefaultMappingFor<SearchEntity>(m => m
                            .IndexName(searchEngineConfiguration.IndexName)
                            .TypeName(searchEngineConfiguration.TypeName));
                })
                .SingleInstance();
            builder.Register(x => new ElasticClient(x.Resolve<ConnectionSettings>()))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}