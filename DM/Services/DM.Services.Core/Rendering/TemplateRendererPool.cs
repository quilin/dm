using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Services.Core.Rendering
{
    /// <inheritdoc cref="ITemplateRendererPool" />
    public class TemplateRendererPool : ITemplateRendererPool, IDisposable
    {
        private bool isDisposed;

        private readonly IDictionary<string, TemplateRenderApplication> pool =
            new ConcurrentDictionary<string, TemplateRenderApplication>();

        /// <inheritdoc />
        public Task<string> Render<TModel>(string templatePath, TModel model)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(TemplateRendererPool));
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

            return application.Render(templatePath, model);
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