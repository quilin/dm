using System.Collections.Generic;
using System.Net.Mime;
using DM.Web.API.Controllers.v1.Account;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DM.Web.API.Authentication
{
    /// <inheritdoc />
    public class AuthenticationSwaggerFilter : IOperationFilter
    {
        /// <inheritdoc />
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Produces = new List<string> {MediaTypeNames.Application.Json};
            if (context.MethodInfo.DeclaringType == typeof(LoginController) &&
                context.MethodInfo.Name == nameof(LoginController.Login))
            {
                return;
            }

            (operation.Parameters ?? (operation.Parameters = new List<IParameter>()))
                .Add(new NonBodyParameter
                {
                    Name = "X-Dm-Auth-Token",
                    In = "header",
                    Type = "string",
                    Required = false,
                    Description = "Authenticated requests require X-Dm-Auth-Token header. " +
                        "You can get the data from POST /account/ method, " +
                        "sending login and password in \"token\" response field"
                });
        }
    }
}