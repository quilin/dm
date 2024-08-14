using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace DM.Services.Core.Extensions;

/// <summary>
/// Расширения построителя веб-приложения
/// </summary>
public static class WebBuilderExtensions
{
    private const int DefaultPort = 5000;

    private static int ExtractPort()
    {
        // For heroku deployment, where only available port is defined in runtime by the environment variable
        var predefinedPort = Environment.GetEnvironmentVariable("PORT");
        return string.IsNullOrEmpty(predefinedPort) || !int.TryParse(predefinedPort, out var port)
            ? DefaultPort
            : port;
    }

    private static IWebHostBuilder UseCustomPort(this IWebHostBuilder builder) => builder
        .UseUrls($"http://+:{ExtractPort()}");

    private static IWebHostBuilder UseCustomGrpcPort(this IWebHostBuilder builder) => builder
        .UseKestrel(options =>
        {
            options.AllowSynchronousIO = true;
            options.ListenAnyIP(ExtractPort(), cfg => cfg.Protocols = HttpProtocols.Http2);
        });

    /// <summary>
    /// Настроить grpc-сервер по-умолчанию
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="TStartup"></typeparam>
    /// <returns></returns>
    public static IWebHostBuilder UseDefaultGrpc<TStartup>(this IWebHostBuilder builder)
        where TStartup : class => builder
        .UseStartup<TStartup>()
        .UseCustomGrpcPort();

    /// <summary>
    /// Настроить веб-сервер по-умолчанию
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="TStartup"></typeparam>
    /// <returns></returns>
    public static IWebHostBuilder UseDefault<TStartup>(this IWebHostBuilder builder)
        where TStartup : class => builder
        .UseStartup<TStartup>()
        .UseCustomPort();
}