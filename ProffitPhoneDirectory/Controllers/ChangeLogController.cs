using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProffitPhoneDirectory.Repositories;

namespace ProffitPhoneDirectory.Controllers;

//[Authorize]
[Route( "api/[controller]" )]
public class ChangeLogController : RootController
{
    protected readonly ChangeLogRepository repository;

    public ChangeLogController( IConfiguration configuration, ChangeLogRepository repository ) : base( configuration )
    {
        this.repository = repository;
    }

    [HttpGet]
    public IActionResult Get( int page = 1, int pageSize = 10/*, DateTime startDate, DateTime endDate*/ ) => Ok( repository.Get( /*startDate, endDate ,*/ page, pageSize) );
}
