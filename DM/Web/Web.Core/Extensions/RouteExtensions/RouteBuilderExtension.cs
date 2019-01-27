using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Web.Core.Extensions.RouteExtensions
{
    public static class RouteBuilderExtension
    {
        private const string ControllerKey = "controller";
        private const string ActionKey = "action";

        public static IRouteBuilder MapAction<T>(this IRouteBuilder routes, string url, Expression<Action<T>> action,
            IDictionary<string, object> defaults = null, object constraints = null)
            where T : Controller
        {
            var actionName = MvcNameOf.Action(action);
            return MapAction<T>(routes, url, actionName, defaults, constraints);
        }

        private static IRouteBuilder MapAction<T>(this IRouteBuilder routes, string url, string actionName,
            IDictionary<string, object> defaults = null, object constraints = null)
            where T : Controller
        {
            var controllerName = MvcNameOf.Controller<T>();

            defaults = defaults ?? new Dictionary<string, object>();
            defaults[ControllerKey] = controllerName;
            defaults[ActionKey] = actionName;

            return routes.MapRoute("", url, defaults, constraints);
        }
    }
}