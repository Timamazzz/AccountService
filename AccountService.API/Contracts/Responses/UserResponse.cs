namespace AccountService.API.Contracts.Responses;

/// <summary>
/// API response containing user data.
/// </summary>
public class UserResponse
{
    /// <summary>
    /// The unique identifier of the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The username of the user.
    /// </summary>
    public string Username { get; set; } = null!;
}