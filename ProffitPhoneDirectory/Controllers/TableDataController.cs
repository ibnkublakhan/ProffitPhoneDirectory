using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProffitPhoneDirectory.Context;
using ProffitPhoneDirectory.Models;
using Microsoft.AspNetCore.Authorization;
using ProffitPhoneDirectory.Repositories;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ProffitPhoneDirectory.Controllers;

[AllowAnonymous]
[Route( "api/[controller]" )]
public class TableDataController : RootController
{
    protected readonly IConfiguration _configuration;
    protected readonly ProffitCorporateInfoContext proffitContext;
    protected readonly DatabaseSingleContext single_context;

    protected readonly EmployeeRepository eRepository;
    protected readonly PositionRepository pRepository;
    protected readonly GroupRepository gRepository;
    protected readonly BranchRepository bRepository;
    
    public TableDataController( IConfiguration configuration, EmployeeRepository eRepository, PositionRepository pRepository, GroupRepository gRepository, BranchRepository bRepository ) : base( configuration )
    {
        this.eRepository = eRepository;
        this.pRepository = pRepository;
        this.gRepository = gRepository;
        this.bRepository = bRepository;

        this.single_context = single_context;
        this.proffitContext = proffitContext;
        this._configuration = _configuration;

    }

    [HttpGet]
    public IActionResult Get()
    {
        List<BranchOutput> branches = bRepository.Get();
        List<ProffitGroup> groups = gRepository.Get().ToList();
        List<EmployeeOutput> employeesPre = eRepository.GetFullInfo().ToList();

        var groupnumber = "";

        IEnumerable<List<object>> employees = employeesPre.Select( e =>
        {
            string ph = GetPhonesFormat( e, groups );

            return new List<object> {
                $"{e.firstname} {e.lastname}",
                e.number,
                ph,
                e.groupnumber,
                e.Position ?? "0",
                e.Branch ?? "0",
            };
        } );

        List<Position>? positions = pRepository.Get();

        return Ok( new { branches, groups, positions, employees } );
    }

    private static IEnumerable<string> GetPhones( EmployeeOutput e, List<ProffitGroup> groups )
    {
        IEnumerable<string?>? ph = groups.FirstOrDefault( x => e.GroupKey == x.Id )
            .phones.Select( y => y.ToString() );

        return ph;
    }

    private static string GetPhonesFormat( EmployeeOutput e, List<ProffitGroup> groups )
    {
        string ph = "";
        var delimiter = ",";

        // убираем разделитель в конце
        ph = new string( ph.Take( ph.Length < 2 ? 0 : ph.Length - delimiter.Length ).ToArray() );

        return ph;
    }
}
