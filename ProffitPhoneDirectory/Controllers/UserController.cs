using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProffitPhoneDirectory.Context;
using ProffitPhoneDirectory.Models;
using ProffitPhoneDirectory.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;

namespace ProffitPhoneDirectory.Controllers;

[Authorize]
[Route( "api/[controller]" )]
public class UserController : RootController
{
    protected readonly UserRepository repository;

    public UserController( IConfiguration configuration, UserRepository repository ) : base( configuration )
    {
        this.repository = repository;
    }

    [HttpDelete]
    public async Task Delete() => repository.Delete( HttpContext.User.Identity.Name );

    [HttpPatch( "username" )]
    public async Task PatchName( [FromBody] string newUsername ) => repository.PatchName( HttpContext.User.Identity.Name, newUsername );

    [HttpPatch( "password" )]
    public async Task PatchPassword( [FromBody] string newPassword ) => repository.PatchPassword( HttpContext.User.Identity.Name, newPassword );

    [HttpPost]
    //[AllowAnonymous]
    public async Task Create( [FromBody] CreateUser newUser ) => repository.Create( newUser );
}
