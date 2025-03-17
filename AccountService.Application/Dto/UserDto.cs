namespace AccountService.Application.Dto;

/// <summary>
/// Data Transfer Object (DTO) for user operations in the service layer.
/// </summary>
public class UserDto
{
    /// <summary>
    /// The unique identifier of the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The username of the user.
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// The hashed password of the user.
    /// </summary>
    public string PasswordHash { get; set; } = null!;
}