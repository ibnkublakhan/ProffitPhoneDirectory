using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProffitPhoneDirectory.Models;
using ProffitPhoneDirectory.Repositories;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace ProffitPhoneDirectory.Controllers;

[Authorize]
//[Route( "api/branch" )]
[Route( "api/[controller]" )]
public class BranchController : RootController
{
    protected readonly BranchRepository repository;

    public BranchController( IConfiguration configuration, BranchRepository repository ) : base( configuration )
    {
        this.repository = repository;
    }

    [HttpGet]
    public IEnumerable<BranchOutput> Get() => repository.Get();

    [HttpPost]
    public async Task Create( string name ) => repository.Create( name );

    [HttpDelete( "{id}" )]
    public async Task Delete( int id ) => repository.Delete( id );

    [HttpPost( "{id}/groups" )]
    public async Task AddGroups( int id, List<int> groupsId ) => repository.AddGroups( id, groupsId ) ;


    [HttpDelete( "{id}/groups" )]
    public async Task DeleteGroups( int id, List<int> groupsId ) => repository.RemoveGroups( id, groupsId ) ;
}
