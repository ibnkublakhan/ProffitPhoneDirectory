using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProffitPhoneDirectory.Repositories;

namespace ProffitPhoneDirectory.Controllers;

[Authorize]
[Route( "api/[controller]" )]
public class EmployeeController : RootController
{
    protected readonly EmployeeRepository repository;

    public EmployeeController( IConfiguration configuration, EmployeeRepository repository ) : base( configuration )
    {
        this.repository = repository;
    }

    [HttpGet]
    public IEnumerable<Models.EmployeeOutput> Get() => repository.GetFullInfoDistinct();

    [HttpPut( "{id}/position" )]
    public async Task SetPosition( int id, int positionId ) => repository.SetPosition( id, positionId );

    [HttpDelete( "{id}/position" )]
    public async Task DeletePosition( int id ) => repository.DeletePosition( id );
}
