using System.Threading.Tasks;

namespace DM.Services.Mail.Rendering.Rendering
{
    /// <summary>
    /// Razor template renderer
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Render template against given model
        /// </summary>
        /// <param name="templateName">Template name</param>
        /// <param name="model">Model</param>
        /// <typeparam name="TModel">Model type</typeparam>
        /// <returns>Rendered template</returns>
        Task<string> Render<TModel>(string templateName, TModel model);
    }
}