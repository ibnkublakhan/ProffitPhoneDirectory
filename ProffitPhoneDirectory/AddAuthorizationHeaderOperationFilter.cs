using Microsoft.OpenApi.Models;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;

namespace ProffitPhoneDirectory;

public class AddAuthorizationHeaderOperationFilter : IOperationFilter
{
    public void Apply( OpenApiOperation operation, OperationFilterContext context )
    {
        if ( context.ApiDescription.TryGetMethodInfo( out MethodInfo methodInfo ) )
        {
            if ( methodInfo.CustomAttributes.Any( attr => attr.AttributeType == typeof( AllowAnonymousAttribute ) ) )
            {
                // Пропуск операции, если у неё есть [AllowAnonymous]
                return;
            }

            // Добавление токена в заголовок
            operation.Parameters ??= new List<OpenApiParameter>();
            operation.Parameters.Add( new OpenApiParameter
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Description = "JWT token",
                Required = false,
                Schema = new OpenApiSchema { Type = "string" }
            } );
        }
    }
}
