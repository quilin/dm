using System.Text.Json;
using System.Threading.Tasks;

namespace DM.Services.Mail.Rendering.Rendering;

/// <inheritdoc />
internal class TemplateRenderer : ITemplateRenderer, IRenderer
{
    /// <inheritdoc />
    public Task<string> Render<TModel>(string templatePath, TModel model) =>
        Task.FromResult(JsonSerializer.Serialize(model));
}