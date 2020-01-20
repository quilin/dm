using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Services.Mail.Rendering.Rendering
{
    /// <inheritdoc />
    internal class Renderer : IRenderer
    {
        private readonly IRenderApplicationPool applicationPool;

        /// <inheritdoc />
        public Renderer(
            IRenderApplicationPool applicationPool)
        {
            this.applicationPool = applicationPool;
        }

        /// <inheritdoc />
        public async Task<string> Render<TModel>(string templateName, TModel model)
        {
            var application = await applicationPool.GetApplication<TModel>();
            var scopeFactory = application.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var renderer = scope.ServiceProvider.GetRequiredService<ITemplateRenderer>();
            return await renderer.Render(templateName, model).ConfigureAwait(false);
        }
    }
}