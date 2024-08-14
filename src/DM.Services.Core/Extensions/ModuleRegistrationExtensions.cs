using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using AutoMapper;
using Microsoft.Extensions.Hosting;

namespace DM.Services.Core.Extensions;

/// <summary>
/// Extensions for autofac registration
/// </summary>
public static class ModuleRegistrationExtensions
{
    private const string RegisteredModulesKey = nameof(RegisteredModulesKey);

    /// <summary>
    /// Register module, but only if not registered
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="TModule"></typeparam>
    /// <returns></returns>
    public static ContainerBuilder RegisterModuleOnce<TModule>(this ContainerBuilder builder)
        where TModule : IModule, new()
    {
        var registeredModules = builder.Properties.TryGetValue(RegisteredModulesKey, out var modulesWrapper) &&
                                modulesWrapper is HashSet<Type> modules
            ? modules
            : new HashSet<Type>();

        if (registeredModules.Contains(typeof(TModule)))
        {
            return builder;
        }

        builder.RegisterModule<TModule>();
        registeredModules.Add(typeof(TModule));
        builder.Properties[RegisteredModulesKey] = registeredModules;
        return builder;
    }

    /// <summary>
    /// Register default types of the calling assembly
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static ContainerBuilder RegisterDefaultTypes(this ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(Assembly.GetCallingAssembly())
            // Only non-abstract classes should be registered automatically
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t =>
            {
                // Classes that only declared private constructors should not be registered
                var declaredConstructors = t.GetDeclaredConstructors();
                return !declaredConstructors.Any() || declaredConstructors.All(d => d.IsPublic);
            })
            // Exceptions should not be registered automatically
            .Where(t => !t.IsSubclassOf(typeof(Exception)))
            .Where(t => !t.IsAssignableTo(typeof(IHostedService)))
            .AsSelf()
            .AsImplementedInterfaces()
            .InstancePerDependency();

        return builder;
    }

    /// <summary>
    /// Register mappings from the calling assembly
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static ContainerBuilder RegisterMapper(this ContainerBuilder builder)
    {
        var callingAssembly = Assembly.GetCallingAssembly();
        builder.RegisterAssemblyTypes(callingAssembly)
            .Where(t => t.IsClass && t.IsSubclassOf(typeof(Profile)))
            .As<Profile>();

        builder
            .Register<IConfigurationProvider>(ctx =>
                new MapperConfiguration(cfg => cfg.AddProfiles(ctx.Resolve<IEnumerable<Profile>>())))
            .SingleInstance();

        builder
            .Register(ctx =>
            {
                var context = ctx.Resolve<IComponentContext>();
                var configuration = context.Resolve<IConfigurationProvider>();
                return configuration.CreateMapper(context.Resolve);
            })
            .InstancePerLifetimeScope();

        return builder;
    }
}