using System.Threading.Tasks;

namespace DM.Services.Core.Rendering
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITemplateRendererPool
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="model"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        Task<string> Render<TModel>(string templatePath, TModel model);
    }
}