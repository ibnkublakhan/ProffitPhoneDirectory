using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProffitPhoneDirectory._3cxModels;
using ProffitPhoneDirectory.Context;
using ProffitPhoneDirectory.Controllers;
using ProffitPhoneDirectory.Models;
using SqlKata;
using System.Net;
using System.Xml.Linq;
//using static ProffitPhoneDirectory.Controllers.ErrorCode;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProffitPhoneDirectory.Repositories;

public class EmployeeRepository : BaseRepository
{
    public EmployeeRepository( ProffitCorporateInfoContext proffitContext, IConfiguration _configuration, DatabaseSingleContext single_context ) : base( proffitContext, _configuration, single_context ) { }

    // вернем сущности  связующие должности с пользователями
    public List<EmployeePosition> EmployeePosition()
    {
        var result = proffitContext.EmployeePosition.Include(ep => ep.Position).ToList()
            .Join(Get(),
                ep => ep.EmployeeId,
                e => e.Id,
                (ep, e) => new EmployeePosition
                {
                    EmployeeId = e.Id,
                    //Employee = e,
                    Position = ep.Position
                }).ToList();
        return result;
    }

    internal void SetPosition( int employeeId, int positionId )
    {
        IEnumerable<Employee> employees = Get();
        if ( !employees.Any( e => e.Id == employeeId ) ) 
            throw new ResponseErrors( $"сотрудника {employeeId} не существует", HttpStatusCode.BadRequest );

        Position? position = proffitContext.Position.FirstOrDefault( e => e.Id == positionId );
        if ( position == null ) 
            throw new ResponseErrors( $"Должность {position} не существует", HttpStatusCode.BadRequest );

        // если нет связи с должностью - добавим
        EmployeePosition? employeePosition = proffitContext.EmployeePosition.FirstOrDefault( e => e.EmployeeId == employeeId );
        if ( employeePosition == null )
        {
            employeePosition = new EmployeePosition { EmployeeId = employeeId };
            proffitContext.EmployeePosition.Add( employeePosition );
        }

        // изменим должность
        employeePosition.Position = position;
        proffitContext.SaveChanges();
    }

    public List<Employee> Get()
    {
        var result = single_context.Ringgroup2dns
            .Join(single_context.Users,
                ringgroup2dn => ringgroup2dn.Fkiddn,
                user => user.Fkidextension,
                (ringgroup2dn, user) => new
                {
                    Ringgroup2dn = ringgroup2dn,
                    User = user
                })
            .Join(single_context.Dns,
                combined => combined.Ringgroup2dn.Fkiddn,
                dn => dn.Iddn,
                (combined, dn) => new Employee
                {
                    Id = combined.User.Iduser,
                    extentionKey = combined.Ringgroup2dn.Fkiddn,
                    firstname = combined.User.Firstname,
                    lastname = combined.User.Lastname,
                    number = dn.Value
                })
            //.Join( EmployeePosition(),
            //    combined => combined.Id,
            //    ep => ep.EmployeeId,
            //    (combined, ep) => new
            //    {
            //        Id = combined.Id,
            //        extentionKey = combined.extentionKey,
            //        firstname = combined.firstname,
            //        lastname = combined.lastname,
            //        number = combined.number,
            //        position = ep.Position

            //    })
            .ToList();
        return result;
    }

    internal void DeletePosition( int employeeId )
    {
        EmployeePosition? employeePosition = proffitContext.EmployeePosition.FirstOrDefault( e => e.EmployeeId == employeeId );
        if ( employeePosition == null ) throw new ResponseErrors( $"сотрудника {employeeId} не существует", HttpStatusCode.BadRequest );

        proffitContext.Remove( employeePosition );
        proffitContext.SaveChanges();
    }

    public IEnumerable<EmployeeOutput> GetFullInfoDistinct() => GetFullInfo().DistinctBy( x => x.Id );
    
    public IEnumerable<EmployeeOutput> GetFullInfo()
    {
        var usersByGroups = single_context.Ringgroup2dns
            .Join(single_context.Users,
            r2 => r2.Fkiddn,
            u => u.Fkidextension,
            (rg2, u) => new EmployeeOutput ( u )
            { 
                ExtentionKey = rg2.Fkiddn,
                GroupKey = rg2.Fkidringgroup
            }
        ).ToList();

        usersByGroups.ForEach( user =>
        {
            var ep = EmployeePosition().FirstOrDefault( x => x.EmployeeId == user.Id );
            var gr = single_context.Ringgroups.Include( x => x.FkiddnNavigation ).FirstOrDefault( x => x.Fkiddn == user.GroupKey );
            List<Ringgroup> grs = single_context.Ringgroups.Include( x => x.FkiddnNavigation ).ToList();
            var grId = gr?.Fkiddn;
            var gb = proffitContext.GroupBranch.Include( x => x.Branch ).FirstOrDefault( x => x.GroupsId == grId );
            var dn = single_context.Dns.FirstOrDefault( x => x.Iddn == user.ExtentionKey );

            user.number = dn?.Value;
            user.Position = ep?.Position?.Id.ToString();
            user.Branch = gb?.Branch?.Id.ToString();
            user.groupname = gr?.Name;
            user.groupnumber = gr?.FkiddnNavigation?.Value;
        } );

        return usersByGroups;
    }
}
