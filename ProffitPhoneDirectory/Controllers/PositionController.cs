using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProffitPhoneDirectory.Models;
using ProffitPhoneDirectory.Repositories;

namespace ProffitPhoneDirectory.Controllers;

[Authorize]
[Route( "api/[controller]" )]
public class PositionController : RootController
{
    protected readonly PositionRepository repository;

    public PositionController( IConfiguration configuration, PositionRepository repository ) : base( configuration )
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<List<Position>> Get() => repository.Get();

    /// <summary>
    /// Создать новую должность
    /// </summary>
    /// <param name="name">Наименование</param>
    /// <param name="color">цвет в HEX</param>
    /// <returns>статус 200</returns>
    [HttpPost]
    public async Task Create( string name, string? color ) => repository.Create( name, color );

    [HttpPut( "{id}" )]
    public async Task Update( int id, string name, string? color ) => repository.Update( id, name, color );

    [HttpPatch( "{id}/name" )]
    public async Task Rename( int id, string name ) => repository.Rename( id, name );

    [HttpPatch( "{id}/color" )]
    public async Task SetColor( int id, string? color ) => repository.SetColor( id, color );

    [HttpDelete( "{id}" )]
    public async Task Delete( int id ) => repository.Delete( id );
}
