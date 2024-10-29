namespace MinimalApiTodoApi.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MinimalApiTodoApi.Helpers;
using MinimalApiTodoApi.Models;
using Microsoft.IdentityModel.Tokens;
using MinimalApiTodoApi.Database;
using Microsoft.EntityFrameworkCore;

public class AuthService
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(string email, string username, string password)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Email == email))
            return (false, "Email is already in use.");

        if (await _dbContext.Users.AnyAsync(u => u.Username == username))
            return (false, "Username is already taken.");

        byte[] passwordkey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Security:PasswordHashSettings:SecretKey") ??
                throw new InvalidOperationException("Valid secretkey is not configured"));

        var user = new User
        {
            Email = email,
            Username = username,
            PasswordHash = PasswordHelper.HashPassword(password,passwordkey),
            Roles = new[] { "User" }
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return (true, "User registered successfully.");
    }

    public async Task<(bool Success, string? Token, string Message)> LoginAsync(string username, string password)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == null)
            return (false, null, "Username not found.");

        byte[] passwordkey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Security:PasswordHashSettings:SecretKey") ??
                throw new InvalidOperationException("Valid secretkey is not configured"));
        if (!PasswordHelper.VerifyPassword(password, user.PasswordHash,passwordkey))
            return (false, null, "Incorrect password.");

        var token = GenerateToken(user);
        return (true, token, "Login successful.");
    }


    public string GenerateToken(User user)
    {
        
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Security:JwtSettings:SecretKey") ??
                throw new InvalidOperationException("Valid secretkey is not configured"));
        Console.WriteLine("key: " + key);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(user),
            Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<double>("Security:JwtSettings:ExpirationMinutes")),
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
