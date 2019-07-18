using System;
using System.Collections.Generic;
using DM.Web.Classic.Controllers;
using DM.Web.Classic.Controllers.CommentariesControllers;
using DM.Web.Classic.Controllers.CommunityControllers;
using DM.Web.Classic.Controllers.ForumControllers;
using DM.Web.Core.Extensions.RouteExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Configuration
{
    public static class RouteConfig
    {
        private static readonly IDictionary<string, object> PagingDefaults = new Dictionary<string, object>
        {
            ["entityNumber"] = 1
        };

        public static void Register(IRouteBuilder routes)
        {
            #region CommentariesRoutes
            routes.MapAction<CommentariesController>("comments/{entityId}/{entityNumber}", c => c.Index(Guid.Empty, 0));
            #endregion
            
            #region ForumRoutes
            routes.MapAction<ForumController>("forum/{forumTitle}/{entityNumber}", c => c.Index(null, 0, null), new Dictionary<string, object>{{"forumTitle", null}, {"entityNumber", 1}});

            routes.MapAction<EditTopicController>("topic/{topicId}/edit", c => c.Edit(Guid.Empty));
            routes.MapAction<TopicController>("topic/{topicId}/{entityNumber}", c => c.Index(Guid.Empty, 0));
            #endregion

            #region CommunityRoutes
            routes.MapAction<CommunityController>("community/{entityNumber}", c => c.Index(0, false), new Dictionary<string, object>{{"entityNumber", 1}, {"withInactive", false}});
            #endregion

            routes.MapAction<ErrorController>("error/{statusCode}", c => c.Index(0, null));

            routes.MapAction<HomeController>("", c => c.Index());
            routes.MapAction<HomeController>("rules", c => c.Rules());
            routes.MapAction<HomeController>("donate", c => c.Donate());

            routes.MapAction<ApiController>("api", c => c.Index());

            routes.MapRoute(
                name: "default_route",
                template: "{controller}/{action}/{id?}",
                defaults: new {controller = "Home", action = "Index"});
        }
    }
}