using DataAccess.Postgres.Migration.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EquipmentInventory.API.Services;

public class AuthorizationService
{
    private readonly IConfiguration? _configuration;

    public AuthorizationService(IConfiguration configuration)
        => _configuration = configuration;

    public string GenerateJwtToken(User user)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]
                ?? throw new Exception("JWT key is empty"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Surname, user.Surname),
                    new Claim(ClaimTypes.Role, user.IdRoleNavigation.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:Expires" ?? throw new Exception("JWT expires is empty")])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return string.Empty;
        }
    }
}
