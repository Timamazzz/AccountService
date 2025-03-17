namespace AccountService.Application.Interfaces.Services;

/// <summary>
/// Interface for password hashing operations.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a plaintext password.
    /// </summary>
    /// <param name="password">The plaintext password.</param>
    /// <returns>The hashed password.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies if a plaintext password matches its hashed counterpart.
    /// </summary>
    /// <param name="password">The plaintext password.</param>
    /// <param name="passwordHash">The hashed password.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    bool VerifyPassword(string password, string passwordHash);
}