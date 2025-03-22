using AccountService.Core.Interfaces.Repositories;
using AccountService.Core.Entities;
using AccountService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccountService.Persistence.Repositories;

/// <summary>
/// Repository for accessing user-related data from the database.
/// </summary>
public class UserRepository(AppDbContext context, ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id)
    {
        logger.LogInformation("Getting user by ID {UserId}", id);

        var user = await context.Users.FindAsync(id);

        if (user == null)
            logger.LogWarning("User with ID {UserId} not found", id);

        return user;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        logger.LogInformation("Getting user by username {Username}", username);

        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            logger.LogWarning("User with username '{Username}' not found", username);

        return user;
    }

    public async Task AddAsync(User user)
    {
        logger.LogInformation("Adding new user {@User}", user);

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        logger.LogInformation("User saved to database with ID {UserId}", user.Id);
    }
}