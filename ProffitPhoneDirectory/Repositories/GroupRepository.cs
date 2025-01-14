using Microsoft.EntityFrameworkCore;
using ProffitPhoneDirectory.Context;
using ProffitPhoneDirectory.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProffitPhoneDirectory.Repositories;

public class GroupRepository : BaseRepository
{
    public GroupRepository( ProffitCorporateInfoContext proffitContext, IConfiguration _configuration, DatabaseSingleContext single_context ) : base( proffitContext, _configuration, single_context )
    {

    }

    private ProffitGroup Get( int Id )
    {
        List<ProffitGroup>? groups = Get();
        ProffitGroup? group = groups.FirstOrDefault( x => x.Id == Id );

        if ( group is null ) throw new Exception( $"группа {Id} не найдена" );
        return group;
    }

    public List<ProffitGroup> Get()
    {
        List<ProffitGroup>? result = single_context.Ringgroups.Include( d => d.FkiddnNavigation )

            //.Select(dn => new
            //{
            //    Id = dn.Iddn,
            //    GroupName = dn.RinggroupFkiddns.FirstOrDefault()?.Name
            //})
            //.ToList()
            .Select( d => new ProffitGroup( d ))
            .ToList();

        if ( result.Any() == false ) throw new Exception( $"не создано ни одной группы" );

        result.ForEach( group =>
        {
            group.phones = new List<PhoneOuter>( proffitContext.PhoneOuter.ToList().Where( x => x.GroupId == group.Id ).ToList() );
        } );

        return result;
    }

    public void AddPhones( int Id, List<PhoneOuterInternal> phonesIn )
    {
        ProffitGroup? group = Get( Id );
        var phones = phonesIn.Select( x => x.GetExternal( group.Id ) );

        //proffitContext.PhoneOuter.ToList().ForEach( x =>
        //{
            //if ( phones.Any( y => x == y ) )
            //    throw new Exception( $"Номер {x} уже был присвоен ранее группе {x.GroupId}" );
        //} );

        proffitContext.PhoneOuter.AddRange( phones );

        proffitContext.SaveChanges();
    }

    public void DeletePhones( int Id, List<PhoneOuterInternal> phonesIn )
    {
        ProffitGroup? group = Get( Id );
        var phones = phonesIn.Select( x => x.GetExternal( group.Id ) );

        List<PhoneOuter>? toDelete = proffitContext.PhoneOuter.ToList().Where( x => x.GroupId == group.Id && phones.Any( y => x == y ) ).ToList();

        toDelete.ForEach( x =>
        {
            proffitContext.PhoneOuter.Remove( x );
        } );

        proffitContext.SaveChanges();
    }
}
