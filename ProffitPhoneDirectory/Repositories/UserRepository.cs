using ProffitPhoneDirectory.Context;
using ProffitPhoneDirectory.Controllers;
using ProffitPhoneDirectory.Models;
using System.Net;
//using static ProffitPhoneDirectory.Controllers.ErrorCode;


namespace ProffitPhoneDirectory.Repositories;

public class UserRepository : BaseRepository
{
    public UserRepository( ProffitCorporateInfoContext proffitContext, IConfiguration _configuration, DatabaseSingleContext single_context ) : base( proffitContext, _configuration, single_context )
    {

    }

    internal void PatchName( string? oldUsername, string? newUsername )
    {
        Users? u = GetUser( oldUsername );

        u.Username = newUsername;
        proffitContext.SaveChanges();
    }

    internal void Delete( string? username )
    {
        Users? u = proffitContext.User.FirstOrDefault( u => u.Username == username );

        proffitContext.User.Remove( u );
        proffitContext.SaveChanges();
    }

    internal void PatchPassword( string? username, string newPassword )
    {
        Users? u = GetUser( username );

        u.PasswordHash = PasswordHasher.HashPassword( newPassword );
        proffitContext.SaveChanges();
    }

    internal void Create( CreateUser newUser )
    {
        if ( newUser.Phone.ToString().Length != 11 ) throw new Exception( "номер должен быть в 11-значном формате" );
        
        //proffitContext.PhoneOuter.Add( new PhoneOuter() );
        var u = new Users
        {
            CreatedAt = DateTime.Now,
            Phone = newUser.Phone,
            IsActive = true,
            Id = Guid.NewGuid(),
            Username = newUser.Username,
            PasswordHash = PasswordHasher.HashPassword( newUser.Password ),
        };

        proffitContext.User.Add( u );
        try
        {
        proffitContext.SaveChanges();

        }
        catch ( Exception e )
        {

            throw;
        }
    }

    private Users GetUser( string? username )
    {
        Users? u = proffitContext.User.FirstOrDefault( u => u.Username == username );
        if ( u == null ) throw new ResponseErrors( $"Пользователя {username} не существует", HttpStatusCode.BadRequest );
        return u;
    }
}
