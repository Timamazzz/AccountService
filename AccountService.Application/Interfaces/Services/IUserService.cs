using AccountService.Application.Dto;

namespace AccountService.Application.Interfaces.Services;

/// <summary>
/// Interface for user service operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The user data transfer object (DTO) or null if not found.</returns>
    Task<UserDto?> GetUserByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>The user data transfer object (DTO) or null if not found.</returns>
    Task<UserDto?> GetUserByUsernameAsync(string username);

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="userDto">The user data transfer object (DTO) containing user details.</param>
    Task CreateUserAsync(UserDto userDto);
}