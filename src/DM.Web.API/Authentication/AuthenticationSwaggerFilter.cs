using System.Collections.Generic;
using System.Reflection;
using DM.Web.API.Controllers.v1.Account;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DM.Web.API.Authentication;

/// <inheritdoc />
public class AuthenticationSwaggerFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.DeclaringType == typeof(LoginController) &&
            context.MethodInfo.Name == nameof(LoginController.Login))
        {
            return;
        }

        (operation.Parameters ?? (operation.Parameters = new List<OpenApiParameter>()))
            .Add(new OpenApiParameter
            {
                Name = ApiCredentialsStorage.HttpAuthTokenHeader,
                In = ParameterLocation.Header,
                Required = context.MethodInfo.GetCustomAttribute<AuthenticationRequiredAttribute>() != null,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                },
                Description = $"Authenticated requests require {ApiCredentialsStorage.HttpAuthTokenHeader} header. " +
                              "You can get the data from POST /account/ method, " +
                              "sending login and password in \"token\" response field"
            });
    }
}