using System;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace DM.Services.Core.Extensions
{
    /// <summary>
    /// Расширения построителя веб-приложения
    /// </summary>
    public static class WebBuilderExtensions
    {
        /// <summary>
        /// Подключиться к заданному через переменные окружения порту
        /// <remarks>Необходимо для Heroku, где порт задается в рантайме и именно такой переменной</remarks>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseCustomPort(this IWebHostBuilder builder)
        {
            // For heroku deployment, where only available port is defined in runtime by the environment variable
            var predefinedPort = Environment.GetEnvironmentVariable("PORT");
            return string.IsNullOrEmpty(predefinedPort)
                ? builder
                : builder.UseUrls($"http://*:{predefinedPort}");
        }

        /// <summary>
        /// Настроить веб-сервер по-умолчанию
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TStartup"></typeparam>
        /// <returns></returns>
        public static IWebHostBuilder UseDefault<TStartup>(this IWebHostBuilder builder)
            where TStartup : class => builder
            .UseKestrel(options => options.AllowSynchronousIO = true)
            .UseSerilog()
            .UseStartup<TStartup>()
            .UseCustomPort();
    }
}