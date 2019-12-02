using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;

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
    }
}