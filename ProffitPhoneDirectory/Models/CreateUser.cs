
using System.ComponentModel;

namespace ProffitPhoneDirectory.Models;

public class CreateUser
{
    [DefaultValue( "admin" )]
    public string Username { get; set; }
    [DefaultValue( "admin" )]
    public string Password { get; set; }
    public int Phone { get; set; }
}
