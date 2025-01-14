
using System.ComponentModel;

namespace ProffitPhoneDirectory.Models;

public class LoginModel
{
    [DefaultValue( "admin" )]
    public string Username { get; set; }
    [DefaultValue( "admin" )]
    public string Password { get; set; }
}
