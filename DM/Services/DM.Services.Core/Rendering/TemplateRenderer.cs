using System.Threading.Tasks;

namespace DM.Services.Core.Rendering
{
    /// <inheritdoc />
    public class TemplateRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public Task<string> Render<TModel>(string templateName, TModel model)
        {
            return Task.FromResult(string.Empty);
        }
    }
}