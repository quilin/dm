using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Services.Mail.Sender
{
    /// <summary>
    /// Extensions for module registration
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds needed configuration for 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddMailSender(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<MailMessagePublishConfiguration>(configuration.GetSection("MailSender").Bind);
            return services;
        }
    }
}