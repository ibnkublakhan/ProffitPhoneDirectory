using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ProffitPhoneDirectory.Models;

public class ChangeLogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public ChangeLogMiddleware( RequestDelegate next, IServiceProvider serviceProvider )
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync( HttpContext context )
    {
        var entityName = GetControllerName(context.Request); // Получите имя сущности
        //var entityName1 = context.Request.RouteValues["controller"] /* as string*/;
        var httpMethod = context.Request.Method; // Получите метод HTTP-запроса (GET, POST и т.д.)
        //логировать прев дата и нью дата
                                                 //var entityName = "e"; // Получите имя сущности
                                                 //var httpMethod = "m"; // Получите метод HTTP-запроса (GET, POST и т.д.)

        using ( var scope = _serviceProvider.CreateScope() )
        {
            var changeLogServiceFactory = scope.ServiceProvider.GetRequiredService<ChangeLogServiceFactory>();
            using ( var changeLogService = changeLogServiceFactory.Create() )
            {
                // Используйте changeLogService для логирования
                //string? username = context.User.Identity.Name;
                string? username = "user";

                if ( httpMethod != "GET" && httpMethod != "OPTIONS" )
                    changeLogService.LogChange( entityName, httpMethod, username );

                try
                {
                    await _next( context );
                }
                catch ( Exception e )
                {
                    if ( e is ResponseErrors re )
                    {
                        context.Response.StatusCode = (int)re.StatusCode;
                        await context.Response.WriteAsync( re.Message );
                    }
                }
            }
        }
    }

    private string GetControllerName( HttpRequest request )
    {
        //var controllerName = request.RouteValues["controller"] as string;
        string Path = request.Path.ToString();
        string[] parts = Path.Split('/');
        string controllerName = "";
        if ( parts.Length > 0 )
            controllerName = parts[parts.Length - 1];

        if ( !string.IsNullOrEmpty( controllerName ) && controllerName.EndsWith( "Controller" ) )
            controllerName = controllerName.Substring( 0, controllerName.Length - 10 ); // Удаление "Controller"

        return controllerName ?? "";
    }
}
