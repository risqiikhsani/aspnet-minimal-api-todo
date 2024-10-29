// Configuration classes
public class SecuritySettings
{
    public JwtSettings JwtSettings { get; set; } = new();
    public PasswordHashSettings PasswordHashSettings { get; set; } = new();
}

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
}

public class PasswordHashSettings
{
    public string SecretKey { get; set; } = string.Empty;
}