using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Services.Core.Rendering
{
    /// <inheritdoc cref="IRenderApplicationPool" />
    internal class RenderApplicationPool : IRenderApplicationPool, IDisposable
    {
        private bool isDisposed;

        private readonly IDictionary<string, TemplateRenderApplication> pool =
            new ConcurrentDictionary<string, TemplateRenderApplication>();

        /// <inheritdoc />
        public Task<ServiceProvider> GetApplication<TModel>()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(RenderApplicationPool));
            }

            var assembly = typeof(TModel).Assembly;
            TemplateRenderApplication application;
            
            lock (pool)
            {
                if (!pool.TryGetValue(assembly.FullName, out application))
                {
                    pool[assembly.FullName] = application = new TemplateRenderApplication(assembly);
                }
            }

            return Task.FromResult(application.ServiceProvider);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
            TemplateRenderApplication[] applications;
            lock (pool)
            {
                applications = pool.Values.ToArray();
                pool.Clear();
            }

            foreach (var application in applications)
            {
                application.Dispose();
            }
        }
    }
}