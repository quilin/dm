using System;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.WebEncoders;

namespace DM.Services.Core.Rendering
{
    /// <summary>
    /// The MVC application to compile and render Razor templates
    /// </summary>
    public class TemplateRenderApplication : IDisposable
    {
        private bool isDisposed;
        private readonly ServiceProvider serviceProvider;
        
        /// <inheritdoc />
        public TemplateRenderApplication(Assembly assembly)
        {
            serviceProvider = CreateServiceProvider(assembly);
        }

        private static ServiceProvider CreateServiceProvider(Assembly assembly)
        {
            var services = new ServiceCollection();
            var provider = new EmbeddedFileProvider(assembly, string.Empty);
            services.AddSingleton<IHostingEnvironment>(new HostingEnvironment
            {
                ApplicationName = assembly.GetName().Name
            });
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Clear();
                options.FileProviders.Add(provider);
            });
            services.Configure<WebEncoderOptions>(options =>
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

            services.AddMvcCore();
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Render template against model
        /// </summary>
        /// <param name="templatePath">Template path</param>
        /// <param name="model">Model</param>
        /// <typeparam name="TModel">Model type</typeparam>
        /// <returns>Rendered template</returns>
        public async Task<string> Render<TModel>(string templatePath, TModel model)
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var renderer = scope.ServiceProvider.GetRequiredService<ITemplateRenderer>();
                return await renderer.Render(templatePath, model).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
            serviceProvider.Dispose();
        }
    }
}