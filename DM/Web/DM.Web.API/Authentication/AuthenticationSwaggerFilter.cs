using System.Collections.Generic;
using DM.Web.API.Controllers.v1.Account;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DM.Web.API.Authentication
{
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
                    Name = "X-Dm-Auth-Token",
                    In = ParameterLocation.Header,
                    Required = false,
                    Description = "Authenticated requests require X-Dm-Auth-Token header. " +
                        "You can get the data from POST /account/ method, " +
                        "sending login and password in \"token\" response field"
                });
        }
    }
}