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
    internal class TemplateRenderer : ITemplateRenderer
    {
        private readonly IRazorViewEngine viewEngine;
        private readonly IServiceProvider serviceProvider;
        private readonly ITempDataProvider tempDataProvider;

        /// <inheritdoc />
        public TemplateRenderer(
            IRazorViewEngine viewEngine,
            IServiceProvider serviceProvider,
            ITempDataProvider tempDataProvider)
        {
            this.viewEngine = viewEngine;
            this.serviceProvider = serviceProvider;
            this.tempDataProvider = tempDataProvider;
        }

        /// <inheritdoc />
        public async Task<string> Render<TModel>(string templatePath, TModel model)
        {
            var httpContext = new DefaultHttpContext {RequestServices = serviceProvider};
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using var writer = new StringWriter();
            var viewResult = viewEngine.FindView(actionContext, templatePath, false);
            if (!viewResult.Success)
            {
                throw new TemplateRenderException(templatePath, viewResult.SearchedLocations);
            }

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var viewContext = new ViewContext(actionContext, viewResult.View, viewData,
                new TempDataDictionary(actionContext.HttpContext, tempDataProvider), writer,
                new HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);
            return writer.ToString();
        }
    }
}