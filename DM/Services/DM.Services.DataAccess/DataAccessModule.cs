using Autofac;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.DataAccess
{
    /// <inheritdoc />
    public class DataAccessModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DmMongoClient>()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterType<UpdateBuilderFactory>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            base.Load(builder);
        }
    }
}