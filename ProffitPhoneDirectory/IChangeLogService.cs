using ProffitPhoneDirectory.Context;
using ProffitPhoneDirectory.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;


public interface IChangeLogService 
{
    void LogChange( string entityName, string action, string username );
}

public class ChangeLogService : IChangeLogService, IDisposable
{
    private readonly ProffitCorporateInfoContext _dbContext;
    private bool _disposed = false;

    public ChangeLogService( ProffitCorporateInfoContext dbContext )
    {
        _dbContext = dbContext;
    }

    public void LogChange( string entityName, string action, string username )
    {
        if ( _disposed )
        {
            throw new ObjectDisposedException( nameof( ChangeLogService ) );
        }

        ChangeLog? changeLogEntry = new ChangeLog
        {
            Id = Guid.NewGuid(), 
            EntityName = entityName,
            Action = action,
            ChangeDate = DateTime.Now,
            User = username,
            PrevData = "PrevData",
            NewData = "PrevData"
        };

        _dbContext.ChangeLogs.Add( changeLogEntry );
        _dbContext.SaveChanges();
    }

    // Метод Dispose, который будет вызываться для освобождения ресурсов
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    // Защищенный метод Dispose для фактического освобождения ресурсов
    protected virtual void Dispose( bool disposing )
    {
        if ( !_disposed )
        {
            if ( disposing )
            {
                // Освободите управляемые ресурсы
                _dbContext.Dispose(); // Здесь вы можете освободить другие управляемые ресурсы, если есть
            }

            // Освободите неуправляемые ресурсы

            _disposed = true;
        }
    }

}

public class ChangeLogServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ChangeLogServiceFactory( IServiceProvider serviceProvider )
    {
        _serviceProvider = serviceProvider;
    }

    public ChangeLogService Create()
    {
        var dbContext = _serviceProvider.GetRequiredService<ProffitCorporateInfoContext>();
        return new ChangeLogService( dbContext );
    }
}