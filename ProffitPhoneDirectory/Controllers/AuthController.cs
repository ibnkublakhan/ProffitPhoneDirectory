using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProffitPhoneDirectory.Context;
using ProffitPhoneDirectory.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProffitPhoneDirectory.Controllers;

[ApiController]
[Route( "api/[controller]" )]

public class AuthController : ControllerBase
{
    protected readonly LoginModel model;
    protected readonly IConfiguration _configuration;
    protected readonly ProffitCorporateInfoContext _context;

    public AuthController( ProffitCorporateInfoContext dbContext, IConfiguration _configuration )
    {
        _context = dbContext;
        this._configuration = _configuration;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<object> Login( [FromBody] LoginModel model )
    {
        var u = _context.User.FirstOrDefault( u => u.Username == model.Username );

        if ( u == null || !PasswordHasher.VerifyPassword( model.Password, u.PasswordHash ) ) return NoContent();

        // сохраним время последней авторизации
        u.LastLogin = DateTime.Now;
        _context.SaveChanges();

        string token = GenerateJwtToken(model.Username);

        return new { token };
    }

    private string GenerateJwtToken( string username )
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            // другие необходимые клеймы
        };

        SymmetricSecurityKey? key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        SigningCredentials? creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken? token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["Jwt:DurationInMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken( token );
    }
}
