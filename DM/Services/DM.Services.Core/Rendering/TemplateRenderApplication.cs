using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using DM.Services.Core.Logging;
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
            var assemblyName = assembly.GetName().Name;
            var applicationName = $"DM.Renderer.{assemblyName}";

            var services = new ServiceCollection();

            var hostingEnvironment = new HostingEnvironment {ApplicationName = assemblyName};
            services
                .AddSingleton<IHostingEnvironment>(hostingEnvironment)
                .AddSingleton<ITemplateRenderer, TemplateRenderer>()
                .AddSingleton<DiagnosticSource>(new DiagnosticListener(applicationName))
                .Configure<RazorViewEngineOptions>(options =>
                {
                    options.FileProviders.Clear();
                    options.FileProviders.Add(new EmbeddedFileProvider(assembly));
                })
                .Configure<WebEncoderOptions>(options =>
                    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

            services
                .AddDmLogging(applicationName)
                .AddMvcCore()
                .AddRazorViewEngine();

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