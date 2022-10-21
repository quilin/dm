using Autofac;
using DM.Services.Core.Configuration;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;

namespace DM.Services.DataAccess;

/// <inheritdoc />
public class DataAccessModule : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(ctx =>
            {
                var connectionString = ctx.Resolve<IOptions<ConnectionStrings>>().Value.Mongo;
                var settings = MongoClientSettings.FromConnectionString(connectionString);
                settings.ClusterConfigurator = cb => cb.Subscribe(
                    new DiagnosticsActivityEventSubscriber(new InstrumentationOptions{ CaptureCommandText = true }));
                return new DmMongoClient(settings, connectionString);
            })
            .AsSelf()
            .AsImplementedInterfaces();

        builder.RegisterType<UpdateBuilderFactory>()
            .AsSelf()
            .AsImplementedInterfaces()
            .SingleInstance();

        base.Load(builder);
    }
}