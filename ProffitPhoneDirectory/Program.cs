using ProffitPhoneDirectory;
using ProffitPhoneDirectory.Context;

internal class Program
{
    private static void Main( string[] args )
    {
        IHost host = Host.CreateDefaultBuilder( args )
            .UseSystemd()
            .UseWindowsService()
            .ConfigureWebHostDefaults( webBuilder => webBuilder.UseStartup<Startup>() )
            .Build();

        CreateDbIfNotExists( host );

        host.Run();
    }

    private static void CreateDbIfNotExists( IHost host )
    {
        using ( IServiceScope scope = host.Services.CreateScope() )
        {
            IServiceProvider services = scope.ServiceProvider;

            DatabaseSingleContext dbcontext = scope.ServiceProvider.GetRequiredService<DatabaseSingleContext>();

            try
            {
                //UserContext context = services.GetRequiredService<UserContext>();
                DatabaseSingleContext context = dbcontext;
                context.Database.EnsureCreated();
            }
            catch ( Exception )
            {
                //ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
                //logger.LogError( ex, "An error occurred creating the DB." );
            }
        }
    }

    public void ConfigureServices( IServiceCollection services )
    {
        services
            .AddScoped<AlertServer, AlertServer>()
        ;
    }
}