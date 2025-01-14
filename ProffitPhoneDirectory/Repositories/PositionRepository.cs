using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProffitPhoneDirectory.Context;
using ProffitPhoneDirectory.Controllers;
using ProffitPhoneDirectory.Models;
using SqlKata;
using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;
//using static ProffitPhoneDirectory.Controllers.ErrorCode;

namespace ProffitPhoneDirectory.Repositories;

public class PositionRepository : BaseRepository
{
    private List<string> colors = new List<string>();

    public PositionRepository( ProffitCorporateInfoContext proffitContext, IConfiguration _configuration, DatabaseSingleContext single_context ) : base( proffitContext, _configuration, single_context ) 
    {
        colors = _configuration["Colors:AntDesignDefault"].Split( ' ' ).ToList();
    }

    public List<Position> Get() => proffitContext.Position.ToList();
    
    internal void Delete( int id )
    {
        Position? position = proffitContext.Position.FirstOrDefault( p => p.Id == id );
        if ( position == null ) throw new ResponseErrors( $"Должность не существует {id}", HttpStatusCode.BadRequest );

        proffitContext.Remove( position );
        proffitContext.SaveChanges();
    }

    internal void Create( string name, string? color )
    {
        if ( proffitContext.Position.Any( p => p.Name == name ) ) throw new ResponseErrors( "уже существует такое имя", System.Net.HttpStatusCode.Forbidden );
        Position? v = new Position { Name = name, Color = color };
        proffitContext.Position.Add( v );
        proffitContext.SaveChanges();
    }

    internal void Update( int id, string name, string? color )
    {
        SetColorNoSave( id, color );
        RenameNoSave( id, name );

        proffitContext.SaveChanges();
    }

    internal void SetColor( int id, string? color )
    {
        SetColorNoSave( id, color );
        proffitContext.SaveChanges();
    }

    internal void Rename( int id, string name )
    {
        RenameNoSave( id, name );
        proffitContext.SaveChanges();
    }

    private bool IsValidHex( string value ) => Regex.IsMatch( value, "^#[0-9A-Fa-f]{3,8}$" );
    private bool IsColor( string value ) => colors.Contains( value );

    private void SetColorNoSave( int id, string? color )
    {
        // если не существует такая должность - ошибка
        var position = proffitContext.Position.FirstOrDefault( e => e.Id == id );
        if ( position == null ) throw new ResponseErrors( $"Должность не существует {id}", HttpStatusCode.BadRequest );

        // может содержать null, hex или предустановленный цвет
        if ( color != null && !IsValidHex( color ?? "" ) && !IsColor( color ?? "" ) ) throw new ResponseErrors( $"Группа не существует {id}", HttpStatusCode.BadRequest );
        
        position.Color = color;
    }

    private void RenameNoSave( int id, string name )
    {
        var pos = proffitContext.Position.FirstOrDefault( p => p.Id == id );
        if ( pos == null ) throw new ResponseErrors( $"Группа не существует {id}", HttpStatusCode.BadRequest );

        pos.Name = name;
    }
}
