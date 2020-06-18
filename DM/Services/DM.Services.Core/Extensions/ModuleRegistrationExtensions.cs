using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using AutoMapper;

namespace DM.Services.Core.Extensions
{
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
                .Where(t => t.IsClass)
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
            builder.RegisterAssemblyTypes(Assembly.GetCallingAssembly())
                .Where(t => t.IsClass && t.IsSubclassOf(typeof(Profile)))
                .As<Profile>();

            builder.Register(ctx => new Mapper(
                    new MapperConfiguration(cfg => cfg.AddProfiles(ctx.Resolve<IEnumerable<Profile>>()))))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            return builder;
        }
    }
}