using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Core.Extensions.RouteExtensions
{
    public static class MvcNameOf
    {
        private const string ControllerSuffix = nameof(Controller);

        public static string Controller<T>() where T : Controller
        {
            var typeName = typeof(T).Name;
            if (!typeName.EndsWith(ControllerSuffix))
            {
                throw new ArgumentException($"Controller type name must end on \"{ControllerSuffix}\"", nameof(T));
            }

            return typeName.Substring(0, typeName.Length - ControllerSuffix.Length);
        }

        public static string Action<T>(Expression<Func<T, IActionResult>> action) where T : Controller
        {
            return ((MethodCallExpression) action.Body).Method.Name;
        }

        public static string Action<T>(Expression<Action<T>> action) where T : Controller
        {
            return ((MethodCallExpression) action.Body).Method.Name;
        }
    }
}