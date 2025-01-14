using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProffitPhoneDirectory.Models;
using ProffitPhoneDirectory.Repositories;
using System.Collections.Generic;

namespace ProffitPhoneDirectory.Controllers;

//[Authorize]
[Route( "api/[controller]" )]
public class GroupController : RootController
{
    protected readonly GroupRepository repository;

    public GroupController( IConfiguration configuration, GroupRepository repository ) : base( configuration )
    {
        this.repository = repository;
    }

    [HttpGet]
    public List<ProffitGroup> Get() => repository.Get();

    [HttpPost( "phones" )]
    public void AddPhones( int Id, List<PhoneOuterInternal> phones ) => repository.AddPhones( Id, phones );

    [HttpDelete( "phones" )]
    public void DeletePhones( int Id, List<PhoneOuterInternal> phones ) => repository.DeletePhones( Id, phones );
}
