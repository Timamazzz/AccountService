using AccountService.Application.Interfaces.Repositories;
using AccountService.Core.Entities;
using AccountService.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id) =>
        await context.Users.FindAsync(id);

    public async Task<User?> GetByUsernameAsync(string username) =>
        await context.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
}