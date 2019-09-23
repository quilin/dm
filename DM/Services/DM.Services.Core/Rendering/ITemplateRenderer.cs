using System.Threading.Tasks;

namespace DM.Services.Core.Rendering
{
    /// <summary>
    /// Razor template renderer
    /// </summary>
    public interface ITemplateRenderer
    {
        /// <summary>
        /// Render template against model
        /// </summary>
        /// <param name="templatePath">Template path</param>
        /// <param name="model">Model</param>
        /// <typeparam name="TModel">Model type</typeparam>
        /// <returns>Rendered template</returns>
        Task<string> Render<TModel>(string templatePath, TModel model);
    }
}