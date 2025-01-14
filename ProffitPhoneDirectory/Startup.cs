using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProffitPhoneDirectory.Context;
using System.Reflection;
using NLog;
using NLog.Web;
using NLog.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Newtonsoft;
using IHttpContextAccessor = Microsoft.AspNetCore.Http.IHttpContextAccessor;
using ProffitPhoneDirectory.Repositories;


namespace ProffitPhoneDirectory;

public class Startup
{
    //#if DEBUG
    //    private const string connectNameProffit = "proffit_connect_test";
    //    private const string connectName3cx = "_3cx_connect_test";
    //#else
    private readonly string connectName3cx = "_3cx_connect";
    private readonly string connectNameProffit = "proffit_connect";
    //#endif
    public readonly IConfiguration Configuration;
    private readonly string? projectName;
    private readonly string? projectVersion;
    private readonly string? connectionString3cx;
    private readonly string? connectionStringProffit;

    public Startup( IConfiguration configuration )
    {
        Configuration = configuration;
        // Получаем сборку, представляющую входную точку приложения
        Assembly entryAssembly = Assembly.GetEntryAssembly();
        // Извлекаем информаицю о проекте из сборки
        projectName = entryAssembly?.GetName().Name;
        projectVersion = $"v{entryAssembly?.GetName().Version}";
        // Получаем строку подключения из файла конфигурации
        connectionString3cx = Configuration.GetConnectionString( connectName3cx );
        connectionStringProffit = Configuration.GetConnectionString( connectNameProffit );
    }

    public void ConfigureServices( IServiceCollection services )
    {
        //    services.AddAuthentication( "Bearer" ).AddJwtBearer( "Bearer", options =>
        //    {
        //        options.Authority = "your_authority"; // URL вашего авторизационного сервера
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateAudience = false // Убедитесь, что аудитория токена соответствует вашему приложению
        //        };
        //    } );
        services.AddScoped<ChangeLogServiceFactory>();
        services.AddTransient<IChangeLogService>( provider =>
        {
            var factory = provider.GetRequiredService<ChangeLogServiceFactory>();
            return factory.Create();
        } );

        services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
            .AddJwtBearer( options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    // получаем URL
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( Configuration["Jwt:Key"] ) )
                };
            } );

        services
        //.AddLogging( loggingBuilder => loggingBuilder
        //    .ClearProviders()
        //    .SetMinimumLevel( Microsoft.Extensions.Logging.LogLevel.Trace ) )
        //    .AddNLog()
        .AddCors()
        // Регистрируем контекст базы данных в контейнере зависимостей
        .AddDbContext<DatabaseSingleContext>( options => options.UseNpgsql( connectionString3cx ) )
        .AddDbContext<ProffitCorporateInfoContext>( options => options.UseNpgsql( connectionStringProffit ) )
        .AddScoped<DatabaseSingleContext, DatabaseSingleContext>()
        .AddScoped<IProffitCorporateInfoContext, ProffitCorporateInfoContext>()
        .AddScoped<EmployeeRepository, EmployeeRepository>()
        .AddScoped<PositionRepository, PositionRepository>()
        .AddScoped<GroupRepository, GroupRepository>()
        .AddScoped<ChangeLogRepository, ChangeLogRepository>()
        .AddScoped<BranchRepository, BranchRepository>()
        .AddScoped<UserRepository, UserRepository>()


        .AddSwaggerGen( c =>
        {
            // Добавьте фильтр авторизации
            c.AddSecurityDefinition( "Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Type = SecuritySchemeType.Http,
                //Name = "MyAuthorization",
                Scheme = "bearer"
            } );

            c.AddSecurityRequirement( new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            } );

            c.SwaggerDoc( "v1", new OpenApiInfo { Title = projectName, Version = projectVersion } );
            //c.AddSwaggerGenNewtonsoftSupport(); // Это для поддержки Newtonsoft.Json

            c.OperationFilter<AddAuthorizationHeaderOperationFilter>(); // Добавляем кастомный фильтр


        } )
        // Завершаем регистрацию сервисов
        .AddControllers();

        // Добавьте поддержку Newtonsoft.Json для Swashbuckle
        services.AddSwaggerGenNewtonsoftSupport();

        //services.AddMvc();
        services.AddControllersWithViews();
        services.AddHttpContextAccessor();

    }

    public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
    {
        app
        .UseMiddleware<ChangeLogMiddleware>()
        /* настройка cors */
        .UseCors( builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            )
        //.AllowCredentials(); // Разрешение отправки учетных данных (например, куки, аутентификация)
        //.WithMethods( "PUT", "POST", "DELETE", "PUTCH" ) ) // Разрешение методов PUT, POST
        .UseSwagger()
        .UseSwaggerUI( c =>
        {
            c.SwaggerEndpoint( "/swagger/v1/swagger.json", projectName );
            c.InjectStylesheet( "/swagger-ui/SwaggerDark.css" );
        } )
        .UseHttpsRedirection()
        .UseRouting()
        .UseAuthentication()
        .UseAuthorization()
        .UseStaticFiles()
        //.UseEndpoints( endpoints => endpoints.MapControllers())
        .UseEndpoints( endpoints => endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}",
            defaults: new { controller = "Home", action = "Index" } ) )
        ;

        //app.UseNLog();

        if ( env.IsDevelopment() ) app.UseDeveloperExceptionPage();
        else app.UseExceptionHandler( "/Home/Error" ).UseHsts();
    }
}
