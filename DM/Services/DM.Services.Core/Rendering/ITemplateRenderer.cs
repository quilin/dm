using System.Threading.Tasks;

namespace DM.Services.Core.Rendering
{
    /// <summary>
    /// Renderer for Razor templates
    /// </summary>
    public interface ITemplateRenderer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="model"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        Task<string> Render<TModel>(string templateName, TModel model);
    }
}