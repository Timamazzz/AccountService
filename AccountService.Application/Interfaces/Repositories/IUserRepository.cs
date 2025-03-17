using AccountService.Core.Entities;

namespace AccountService.Application.Interfaces.Repositories;

/// <summary>
/// Interface for user data operations in the repository layer.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The user entity or null if not found.</returns>
    Task<User?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>The user entity or null if not found.</returns>
    Task<User?> GetByUsernameAsync(string username);

    /// <summary>
    /// Adds a new user to the system.
    /// </summary>
    /// <param name="user">The user entity to be stored.</param>
    Task AddAsync(User user);
}