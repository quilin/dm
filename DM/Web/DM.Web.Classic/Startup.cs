using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DM.Services.Core.Configuration;
using DM.Services.DataAccess;
using DM.Services.DataAccess.MongoIntegration;
using DM.Web.Classic.Middleware;
using DM.Web.Core.Binders;
using DM.Web.Core.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Web.Classic
{
    public class Startup
    {
        protected IConfiguration Configuration { get; set; }
        
        private static Assembly[] GetAssemblies()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies().Select(Assembly.Load).ToArray();
            return referencedAssemblies
                .Union(new[] {currentAssembly})
                .Union(referencedAssemblies.SelectMany(a => a.GetReferencedAssemblies().Select(Assembly.Load)))
                .Where(a => a.FullName.StartsWith("DM."))
                .Distinct()
                .ToArray();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddOptions()
                .AddMemoryCache()
                .AddDbContext<DmDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString(nameof(DmDbContext))))
                .Configure<ConnectionStrings>(Configuration.GetSection(nameof(ConnectionStrings)).Bind)
                .Configure<IntegrationSettings>(Configuration.GetSection(nameof(IntegrationSettings)).Bind)
                .AddMvc(config => config.ModelBinderProviders.Insert(0, new ReadableGuidBinderProvider()))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var containerBuilder = new ContainerBuilder();
            var assemblies = GetAssemblies();
            containerBuilder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.IsClass)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            containerBuilder.RegisterType<DmMongoClient>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            containerBuilder.Populate(services);

            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder appBuilder, IHostingEnvironment env)
        {
            appBuilder
                .UseMiddleware<ErrorHandlingMiddleware>()
                .UseMiddleware<AuthenticationMiddleware>();
            appBuilder
                .UseHsts()
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseCookiePolicy()
                .UseMvc();
        }
    }
}