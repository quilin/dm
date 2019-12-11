using System;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
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
    internal class TemplateRenderApplication : IDisposable
    {
        private bool isDisposed;

        /// <inheritdoc />
        public TemplateRenderApplication(Assembly assembly)
        {
            ServiceProvider = CreateServiceProvider(assembly);
        }

        /// <summary>
        /// Get service provider
        /// </summary>
        public ServiceProvider ServiceProvider { get; }

        private static ServiceProvider CreateServiceProvider(Assembly assembly)
        {
            var services = new ServiceCollection();
            var provider = new EmbeddedFileProvider(assembly, string.Empty);
            services.AddSingleton<IHostingEnvironment>(new HostingEnvironment
            {
                ApplicationName = assembly.GetName().Name
            });
            services.AddSingleton<ITemplateRenderer, TemplateRenderer>();
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

        /// <inheritdoc />
        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
            ServiceProvider.Dispose();
        }
    }
}