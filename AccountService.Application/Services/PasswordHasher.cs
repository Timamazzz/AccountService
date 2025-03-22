using AccountService.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace AccountService.Application.Services;

public class PasswordHasher(ILogger<PasswordHasher> logger) : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        var result = BCrypt.Net.BCrypt.Verify(password, passwordHash);

        if (!result)
            logger.LogWarning("Password verification failed.");

        return result;
    }
}