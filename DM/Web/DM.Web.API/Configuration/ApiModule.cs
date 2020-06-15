using System.Reflection;
using Autofac;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Extensions;
using DM.Services.Core.Parsing;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.MessageQueuing;
using DM.Services.Search;
using Microsoft.AspNetCore.Http;
using Module = Autofac.Module;

namespace DM.Web.API.Configuration
{
    /// <inheritdoc />
    public class ApiModule : Module
    {
        private readonly Assembly[] assemblies;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBbParserProvider bbParserProvider;

        /// <inheritdoc />
        public ApiModule(
            Assembly[] assemblies,
            IHttpContextAccessor httpContextAccessor,
            IBbParserProvider bbParserProvider)
        {
            this.assemblies = assemblies;
            this.httpContextAccessor = httpContextAccessor;
            this.bbParserProvider = bbParserProvider;
        }

        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.IsClass)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterType<IdentityProvider>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterTypes(typeof(DmMongoClient))
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterInstance(httpContextAccessor)
                .AsSelf()
                .AsImplementedInterfaces();
            builder.RegisterInstance(bbParserProvider)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterModuleOnce<MessageQueuingModule>();
            builder.RegisterModuleOnce<SearchEngineModule>();
        }
    }
}