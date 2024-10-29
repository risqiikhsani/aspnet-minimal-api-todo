namespace MinimalApiTodoApi.Helpers;

using System.Security.Cryptography;
using System.Text;

public class PasswordHelper
{
    public static string HashPassword(string password,byte[] _key)
    {
        using var hmac = new HMACSHA256(_key);
        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(passwordHash);
    }

    public static bool VerifyPassword(string password, string hashedPassword,byte[] _key)
    {
        using var hmac = new HMACSHA256(_key);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(computedHash) == hashedPassword;
    }
}
