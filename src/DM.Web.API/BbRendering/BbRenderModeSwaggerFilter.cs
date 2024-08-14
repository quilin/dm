using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DM.Web.API.BbRendering;

/// <inheritdoc />
internal class BbRenderModeSwaggerFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        (operation.Parameters ?? (operation.Parameters = new List<OpenApiParameter>()))
            .Add(new OpenApiParameter
            {
                Name = "X-Dm-Bb-Render-Mode",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Description = "Requests with user defined texts that allows usage of BB-codes may be" +
                              $" rendered differently by passing the {"X-Dm-Bb-Render-Mode"} header" +
                              $" of one of following values {string.Join(", ", Enum.GetValues(typeof(BbRenderMode)).Cast<BbRenderMode>())}"
            });
    }
}