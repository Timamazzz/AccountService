using AutoMapper;
using AccountService.Application.Dto;
using AccountService.Application.Exceptions;
using AccountService.Core.Interfaces.Repositories;
using AccountService.Application.Interfaces.Services;
using AccountService.Core.Entities;
using Microsoft.Extensions.Logging;

namespace AccountService.Application.Services;

/// <summary>
/// Service for managing user operations.
/// </summary>
public class UserService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IMapper mapper,
    ILogger<UserService> logger) : IUserService
{
    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        logger.LogInformation("Fetching user by ID: {UserId}", id);

        var user = await userRepository.GetByIdAsync(id);

        if (user == null)
        {
            logger.LogWarning("User with ID {UserId} not found", id);
            throw new NotFoundException("User", id);
        }

        return mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        logger.LogInformation("Fetching user by username: {Username}", username);

        var user = await userRepository.GetByUsernameAsync(username);

        if (user == null)
        {
            logger.LogWarning("User with username '{Username}' not found", username);
            throw new NotFoundException("User", username);
        }

        return mapper.Map<UserDto>(user);
    }

    public async Task CreateUserAsync(UserDto userDto)
    {
        logger.LogInformation("Attempting to create user with username: {Username}", userDto.Username);

        var existingUser = await userRepository.GetByUsernameAsync(userDto.Username);
        if (existingUser != null)
        {
            logger.LogWarning("Username '{Username}' is already taken", userDto.Username);
            throw new IsTakenException("A user with this username already exists", errorCode: "username.is_taken");
        }

        userDto.PasswordHash = passwordHasher.HashPassword(userDto.PasswordHash);

        var user = mapper.Map<User>(userDto);
        await userRepository.AddAsync(user);

        logger.LogInformation("User created successfully with ID: {UserId}", user.Id);
    }
}