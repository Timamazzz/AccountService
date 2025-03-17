namespace AccountService.API.Contracts.Requests;

/// <summary>
/// Request model for creating a user.
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// The username of the new user.
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// The password of the new user.
    /// </summary>
    public string Password { get; set; } = null!;
}