using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Services.Mail.Rendering.Rendering
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRenderApplicationPool
    {
        /// <summary>
        /// Gets application service provider from pool
        /// </summary>
        /// <typeparam name="TModel">Template model type</typeparam>
        /// <returns>Service provider to get renderer from</returns>
        Task<ServiceProvider> GetApplication<TModel>();
    }
}