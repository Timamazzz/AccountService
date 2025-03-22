using AutoMapper;
using AccountService.Application.Dto;
using AccountService.Application.Exceptions;
using AccountService.Core.Interfaces.Repositories;
using AccountService.Application.Interfaces.Services;
using AccountService.Core.Entities;

namespace AccountService.Application.Services;

/// <summary>
/// Service for managing user operations.
/// </summary>
public class UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper mapper) : IUserService
{
    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null)
            throw new NotFoundException("User", id);

        return mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user == null)
            throw new NotFoundException("User", username);

        return mapper.Map<UserDto>(user);
    }

    public async Task CreateUserAsync(UserDto userDto)
    {
        var existingUser = await userRepository.GetByUsernameAsync(userDto.Username);
        if (existingUser != null)
            throw new ConflictException("A user with this username already exists");

        userDto.PasswordHash = passwordHasher.HashPassword(userDto.PasswordHash);
        var user = mapper.Map<User>(userDto);
        await userRepository.AddAsync(user);
    }
}