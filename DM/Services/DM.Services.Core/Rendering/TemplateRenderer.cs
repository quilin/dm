using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace DM.Services.Core.Rendering
{
    /// <inheritdoc />
    public class TemplateRenderer : ITemplateRenderer
    {
        private readonly IRazorViewEngine viewEngine;
        private readonly ITempDataProvider tempDataProvider;
        private readonly IServiceProvider serviceProvider;

        /// <inheritdoc />
        public TemplateRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            this.viewEngine = viewEngine;
            this.tempDataProvider = tempDataProvider;
            this.serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public async Task<string> Render<TModel>(string templateName, TModel model)
        {
            var httpContext = new DefaultHttpContext {RequestServices = serviceProvider};
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
            using (var output = new StringWriter())
            {
                var findViewResult = viewEngine.FindView(actionContext, templateName, true);
                if (!findViewResult.Success)
                {
                    throw new TemplateNotFoundException(templateName, findViewResult.SearchedLocations);
                }

                var view = findViewResult.View;
                var viewDictionary = new ViewDataDictionary(
                    new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(actionContext, findViewResult.View, viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                    output, new HtmlHelperOptions());

                await view.RenderAsync(viewContext);
                return output.ToString();
            }
        }
    }
}