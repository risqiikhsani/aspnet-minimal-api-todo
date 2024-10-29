namespace MinimalApiTodoApi.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MinimalApiTodoApi.Helpers;
using MinimalApiTodoApi.Models;
using Microsoft.IdentityModel.Tokens;


public class AuthService
{
    public string GenerateToken(User user)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(user),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = credentials,
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name, user.Email));
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

        foreach (var role in user.Roles)
            claims.AddClaim(new Claim(ClaimTypes.Role, role));

        return claims;
    }
}
