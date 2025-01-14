using ProffitPhoneDirectory._3cxModels;
using ProffitPhoneDirectory.Context;
using ProffitPhoneDirectory.Models;
using System.Linq;

namespace ProffitPhoneDirectory.Repositories;


//[Route( "api/_3cx" )]
public class ChangeLogRepository : BaseRepository
{

    public ChangeLogRepository( ProffitCorporateInfoContext proffitContext, IConfiguration _configuration, DatabaseSingleContext single_context ) : base( proffitContext, _configuration, single_context )
    {
    }

    //public object Get( int start = -1, int end = 0 )
    public LogOutput Get( int page, int pageSize/*, DateTime startDate, DateTime endDate*/ )
    {
        //try
        //{
        // Ваша логика обработки пагинации
        //}
        //catch ( Exception ex )
        //{
        //    return BadRequest( ex.Message );
        //}

        //Определите начало и конец периода
        DateTime startDate = DateTime.UtcNow.Date.AddDays( -1 );
        DateTime endDate = DateTime.UtcNow.Date.AddDays( 0 );

        IEnumerable<_3cxModels.AuditLog> cx = single_context.AuditLogs/*.Where( record => record.TimeStamp >= startDate && record.TimeStamp <= endDate )*/;
        IEnumerable<ChangeLog>? pr = proffitContext.ChangeLogs/*.Where( record => record.ChangeDate >= startDate && record.ChangeDate <= endDate )*/;

        //var items = _items.Skip((page - 1) * pageSize).Take(pageSize);
        //return Ok( items );

        // Ваша логика обработки пагинации
        IEnumerable<AuditLogInternal>? items2 = cx
            .OrderBy( x => x.TimeStamp )
            .Select( x => new AuditLogInternal(x) )
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        IEnumerable<ChangeLogInternal>? items = pr
            .OrderBy( x => x.ChangeDate )
            .Select( x => new ChangeLogInternal(x) )
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        List<LogInternalBase> itemsAll = new List<LogInternalBase>();
        itemsAll.AddRange( items );
        itemsAll.AddRange( items2 );

        itemsAll.OrderBy( x => x.ChangeDate );

        LogOutput r = new LogOutput
        (
            startDate,
            endDate,
            itemsAll
            //items2
        );

        return r;
    }
}


public class LogOutput
{
    public LogOutput( DateTime startDate, DateTime endDate, /*IEnumerable<ChangeLog> proffit,*/ IEnumerable<LogInternalBase> log )
    {
        StartDate = startDate;
        EndDate = endDate;
        //Proffit = proffit;
        this.Log = log;
    }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    //public IEnumerable<ChangeLog> Proffit { get; set; }
    //public IEnumerable<AuditLogInternal> _3cx { get; set; }
    public IEnumerable<LogInternalBase> Log { get; set; }
}