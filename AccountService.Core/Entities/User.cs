namespace AccountService.Core.Entities;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User
{
    /// <summary>
    /// The unique identifier of the user.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// The username (login) of the user.
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// The hashed password of the user.
    /// </summary>
    public string PasswordHash { get; set; } = null!;
}