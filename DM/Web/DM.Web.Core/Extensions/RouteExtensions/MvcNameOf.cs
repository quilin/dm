using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Core.Extensions.RouteExtensions
{
    /// <summary>
    /// Extension class for MVC routing
    /// </summary>
    public static class MvcNameOf
    {
        private const string ControllerSuffix = nameof(Controller);

        /// <summary>
        /// Get controller name out of controller type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string Controller<T>() where T : Controller
        {
            var typeName = typeof(T).Name;
            if (!typeName.EndsWith(ControllerSuffix))
            {
                throw new ArgumentException($"Controller type name must end on \"{ControllerSuffix}\"", nameof(T));
            }

            return typeName.Substring(0, typeName.Length - ControllerSuffix.Length);
        }

        /// <summary>
        /// Get action name out of action type
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string Action<T>(Expression<Func<T, IActionResult>> action) where T : Controller
        {
            return ((MethodCallExpression) action.Body).Method.Name;
        }

        /// <summary>
        /// Get action name out of action type
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string Action<T>(Expression<Action<T>> action) where T : Controller
        {
            return ((MethodCallExpression) action.Body).Method.Name;
        }
    }
}